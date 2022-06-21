using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitAPI_FinalTask
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;        


            Transaction transaction = new Transaction(doc,"Помещения");
            transaction.Start();
            List<Level> levelsList = new FilteredElementCollector(doc)
                 .OfClass(typeof(Level))
                 .OfType<Level>()
                 .ToList();
            foreach (Level level in levelsList)
            {
                PlanTopology topology = doc.get_PlanTopology(level);
                PlanCircuitSet circuitSet = topology.Circuits;
                string l = level.Name.Substring(level.Name.IndexOf(" ") + 1);
                int i = 1;
                foreach (PlanCircuit planCircuit in circuitSet)
                {
                    if (planCircuit == null) continue;
                    Room room = doc.Create.NewRoom(null, planCircuit);
                    room.get_Parameter(BuiltInParameter.ROOM_NUMBER).Set(l + "." + i.ToString());
                    i++;
                }
            }

            transaction.Commit();
            return Result.Succeeded;
        }
    }
}
