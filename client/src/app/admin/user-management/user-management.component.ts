import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_services/admin.service';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent implements OnInit {
  users: User[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();   //referencja do naszego comp.. modalnego Role
  availableRoles = [
    'Admin',
    'Moderator',
    'Member'
  ]

  constructor( private adminService: AdminService,private modalService: BsModalService) { }

  ngOnInit(): void {
    this.getUsersWithRoles();
  }

  getUsersWithRoles(){
    this.adminService.getUsersWithRoles().subscribe({
      next: users => this.users = users
    })
  }

  openRolesModal(user: User){
    const config = {
      class:'modal-dialog-centered',
      initialState: {
        username: user.username,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next:  () => { // nie chcemy wysłać żądania do API jeśli role są takie same jak u usera,np. otworzył modal i zdecydował nie zmienić ról
          const selectedRoles = this.bsModalRef.content?.selectedRoles;
          if(!this.arrayEqual(selectedRoles!, user.roles)) {
            this.adminService.updateUserRoles(user.username, selectedRoles!.join(',')).subscribe({
              next: roles => user.roles = roles
            })

          }
      }
    })
   }
  
   private arrayEqual(arr1: any, arr2: any){ // porównanie tablic z rolami czy są takie sam
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());

   }

}
