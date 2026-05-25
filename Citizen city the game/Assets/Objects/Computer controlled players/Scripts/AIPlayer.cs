using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading.Tasks;

public class AIPlayer
{
    public Player self;
    public List<Player> others;

    private GameController gameController;
    private ProjectController projectController;
    private DataSpacesController dataSpacesController;
    private ConsultantManager consultantManager;
    private int ThinkingDelay;

    public void Initialize(Player self, List<Player> others, GameController gameController, ProjectController projectController, DataSpacesController dataSpacesController, ConsultantManager consultantManager, int ThinkingDelay)
    {
        this.self = self;
        this.others = others;
        this.gameController = gameController;
        this.projectController = projectController;
        this.dataSpacesController = dataSpacesController;
        this.consultantManager = consultantManager;
        this.ThinkingDelay = ThinkingDelay;
    }

    public async Task TakeTurn()
    {
        Debug.Log("AIPlayer TakeTurn: " + self.name);

        // 1) Observe other players' dataspace progress and try to mirror actions when beneficial
        foreach (Player other in others)
        {
            if (other == self) continue;

            foreach (DataSpaceData otherSpace in other.DataSpaces)
            {
                foreach (DataSpaceDataRequired req in otherSpace.neededData)
                {
                    // if another player has contributed a card towards a dataspace that requires this AI's color
                    if (req.color == self.color && req.isMet)
                    {
                        // try to find matching requirement in this AI's own dataspaces
                        foreach (DataSpaceData mySpace in self.DataSpaces)
                        {
                            DataSpaceDataRequired myReq = mySpace.neededData.Find(d => d.cardType == req.cardType && d.color != self.color && !d.isMet);
                            if (myReq != null)
                            {
                                // if AI has a matching card, play it
                                DataCard card = self.cards.Find(c => c.CardType == myReq.cardType && c.Color == myReq.color);
                                if (card != null)
                                {
                                    dataSpacesController.CheckPlayerCards(myReq);
                                    dataSpacesController.EnableDataSpace(mySpace.id);
                                }
                            }
                        }
                    }
                }
            }
        }

        await Task.Delay(ThinkingDelay);
        Debug.Log("AIPlayer " + self.name + " is taking action on its own dataspaces.");

        // 2) Try to progress own dataspaces if AI has required cards (random chance to start)
        foreach (DataSpaceData mySpace in self.DataSpaces)
        {
            bool municipaldataspace = mySpace.neededData.Count == 3;

            // Determine the AI's "intent" for this specific dataspace this turn
            float baseChance = municipaldataspace ? 1f : 1f;

            // TODO: add some sort of check that makes it very likely to finish a dataspace if it has all the cards it needs for it

            if (Random.value < baseChance)
            {
                foreach (DataSpaceDataRequired req in mySpace.neededData)
                {
                    if ((!req.isMet && req.color != self.color && !municipaldataspace) || (!req.isMet && municipaldataspace))
                    {
                        DataCard card = self.cards.Find(c => c.CardType == req.cardType && c.Color == req.color);
                        if (card != null)
                        {
                            dataSpacesController.CheckPlayerCards(req);
                        }
                    }
                }
                dataSpacesController.EnableDataSpace(mySpace.id);
            }

            // If this is a municipal dataspace and AI still misses cards for it, try hiring a consultant specifically for this dataspace
            if (municipaldataspace)
            {
                HashSet<DataCardType> municipalNeeded = new HashSet<DataCardType>();
                foreach (DataSpaceDataRequired req in mySpace.neededData)
                {
                    if (!req.isMet)
                    {
                        bool hasCard = self.cards.Any(c => c.CardType == req.cardType && c.Color == req.color);
                        if (!hasCard) municipalNeeded.Add(req.cardType);
                    }
                }

                TryHireConsultantForNeededTypes(municipalNeeded);
            }
        }

        await Task.Delay(ThinkingDelay);
        Debug.Log("AIPlayer " + self.name + " is considering its projects.");

        // 3) Try to satisfy project requirements when possible
        foreach (ProjectData project in self.PersonalProjects)
        {
            foreach (DataRequired req in project.NeededData)
            {
                if (!req.IsMet)
                {
                    DataCard card = self.cards.Find(c => c.CardType == req.CardType && c.Color == req.Color);
                    if (card != null)
                    {
                        projectController.CheckPlayerCards(req);
                    }
                }
            }
        }

        foreach (ProjectData project in self.ProvicialProjects)
        {
            foreach (DataRequired req in project.NeededData)
            {
                if (!req.IsMet)
                {
                    DataCard card = self.cards.Find(c => c.CardType == req.CardType && c.Color == req.Color);
                    if (card != null)
                    {
                        projectController.CheckPlayerCards(req);
                    }
                }
            }
        }

        await Task.Delay(ThinkingDelay);
        Debug.Log("AIPlayer " + self.name + " is considering hiring consultants for missing data.");

        // 4) Consider hiring consultants for general missing data (projects / non-municipal dataspaces)
        // collect missing data card types (that AI doesn't already have)
        HashSet<DataCardType> generalNeeded = new HashSet<DataCardType>();

        foreach (ProjectData project in self.PersonalProjects.Concat(self.ProvicialProjects))
        {
            foreach (DataRequired req in project.NeededData)
            {
                if (!req.IsMet)
                {
                    bool hasCard = self.cards.Any(c => c.CardType == req.CardType && c.Color == req.Color);
                    if (!hasCard) generalNeeded.Add(req.CardType);
                }
            }
        }

        foreach (DataSpaceData ds in self.DataSpaces)
        {
            // skip municipal dataspaces here since we handled them above
            if (ds.neededData.Count == 3) continue;

            foreach (DataSpaceDataRequired req in ds.neededData)
            {
                if (!req.isMet)
                {
                    bool hasCard = self.cards.Any(c => c.CardType == req.cardType && c.Color == req.color);
                    if (!hasCard) generalNeeded.Add(req.cardType);
                }
            }
        }

        TryHireConsultantForNeededTypes(generalNeeded);

        // End turn
        gameController.EndTurn();
    }

    private void TryHireConsultantForNeededTypes(HashSet<DataCardType> neededTypes)
    {
        if (consultantManager == null) return;
        if (neededTypes == null || neededTypes.Count == 0) return;

        Consultant chosen = null;
        foreach (Consultant c in consultantManager.consultantOptions)
        {
            if (self.money < c.BasePrice) continue;
            if (c.Specializations.Any(s => neededTypes.Contains(s)))
            {
                chosen = c;
                break;
            }
        }

        if (chosen == null) return;

        List<DataCard> selection = new List<DataCard>();
        foreach (DataCardType cardType in chosen.Specializations)
        {
            DataCard dataCard = new DataCard(cardType, self.color, false);
            selection.Add(dataCard);
        }

        chosen.HireConsultant(selection, self);
    }

    public bool TradingDecision()
    {
        return Random.Range(0,1) < 0.3; // TODO add actual trading logic here
    } 
}
