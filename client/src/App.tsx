import { PivotViewComponent } from '@syncfusion/ej2-react-pivotview';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';
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
    dataSource: oData,
    expandAll: true,
    rows: [{ name: 'shipCity', caption: 'Ship City' }],
    columns: [{ name: 'customerID', caption: 'Customer ID' }],
    values: [{ name: 'freight', caption: 'Freight' }],
    filters: [],
    fieldMapping: [{ name: 'employeeID', caption: 'Employee ID' }, { name: 'orderID', caption: 'Order ID' }, ]
  }
  const editSettings = { allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  
  function beginDrillThrough(args: any) {
    for (var i = 0; i < args.gridObj.columns.length; i++) {
      if (args.gridObj.columns[i].field == "orderID") {
        // Use the unique field as the primary key
        // Primary key ensures correct record targeting for CRUD operations
        args.gridObj.columns[i].isPrimaryKey = true;
      } else {
        // By default, only report-bound fields appear in the editing UI
        // Make all fields visible so they can be edited if required
        args.gridObj.columns[i].visible = true;
      }
    }
  }
  return (<PivotViewComponent id='PivotView' ref={(scope: any) => { pivotObj = scope; }} height={350} dataSourceSettings={dataSourceSettings} editSettings={editSettings} load={load} beginDrillThrough={beginDrillThrough}></PivotViewComponent>);
};

export default App;
