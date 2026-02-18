import { PivotViewComponent } from '@syncfusion/ej2-react-pivotview';
import { DataManager, Query, UrlAdaptor } from '@syncfusion/ej2-data';
import './App.css';

function App() {
  let pivotObj: PivotViewComponent;

  let oData: DataManager = new DataManager({
    url: 'https://localhost:7284/api/Pivot',
    insertUrl: 'https://localhost:7284/api/Pivot/Insert',
    updateUrl: 'https://localhost:7284/api/Pivot/Update',
    removeUrl: 'https://localhost:7284/api/Pivot/Remove',
    adaptor: new UrlAdaptor
  });
  const dataSourceSettings = {
    dataSource: [],
    expandAll: true,
    rows: [{ name: 'shipCity' }],
    columns: [{ name: 'customerID' }],
    values: [{ name: 'freight' }],
    filters: [],
  }
  const editSettings = { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  function load() {
    oData.executeQuery(new Query())
      .then((e: any) => {
        pivotObj.dataSourceSettings.dataSource = e.result.result;
      });
  }
  function beginDrillThrough(args: any) {
    for (var i = 0; i < args.gridObj.columns.length; i++) {
      if (args.gridObj.columns[i].field !== "orderID") {
        args.gridObj.columns[i].visible = true;
      }
    }
    args.gridObj.addEventListener('actionBegin', gridActionBegin);
  }

  function gridActionBegin(args: any) {
    if (args.action == "add" && args.requestType == "save") {
      oData.insert(args.data);
    } else if (args.action == "edit" && args.requestType == "save") {
      oData.update("OrderID", args.data);
    } else if (args.requestType == "delete") {
      const id = args.data[0].orderID;
      oData.remove("orderID", id);
    }
  }
  return (<PivotViewComponent id='PivotView' ref={(scope: any) => { pivotObj = scope; }} height={350} dataSourceSettings={dataSourceSettings} editSettings={editSettings} load={load} beginDrillThrough={beginDrillThrough}></PivotViewComponent>);
};

export default App;