import { Component, Inject, OnInit } from '@angular/core';
import { EditSettingsModel, ToolbarItems, SelectionSettingsModel } from '@syncfusion/ej2-angular-treegrid';
import { DataManager, UrlAdaptor } from '@syncfusion/ej2-data';

@Component({
  selector: 'app-fetch-data-component',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {

  public editSettings: EditSettingsModel;
  public toolbar: ToolbarItems[];
  public data: DataManager;
  public selectionSettings: SelectionSettingsModel;

  ngOnInit(): void {
    this.editSettings = { allowEditing: true, allowAdding: true, allowDeleting: true, mode: "Row", newRowPosition: "Below" };
    this.toolbar = ["Add", "Edit", "Delete", "Update", "Cancel"];
    this.selectionSettings = { type: "Multiple" };
    this.data = new DataManager({
      url: "api/Tasks/DataSource",
      insertUrl: "api/Tasks/Insert",
      updateUrl: "api/Tasks/Update",
      removeUrl: "api/Tasks/Remove",
      batchUrl:"api/Tasks/BatchDelete",
      adaptor: new UrlAdaptor
    });
  }
}

