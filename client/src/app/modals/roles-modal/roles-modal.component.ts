import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent  {
  username = '';
  availableRoles: any[] = [];
  selectedRoles : any[] = [];
 

  constructor(public bsModalRef: BsModalRef) { }

  

  updateChecked(checkedValue:string){
    const index = this.selectedRoles.indexOf(checkedValue); //if index = -1 - oznacza ,że nie znajduję się ona wewnątrz tablicy selectedRoles
    index !== -1 ? this.selectedRoles.splice(index, 1) : this.selectedRoles.push(checkedValue); //usuniemy 1 element przy tym indeksie 
  }

}
