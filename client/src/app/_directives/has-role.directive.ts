import { Directive, Input, OnInit, TemplateRef, ViewContainerRef } from '@angular/core';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';
import { take } from 'rxjs';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective  implements OnInit{
  @Input() appHasRole: string[] = [];
  user: User = {} as User;



  constructor(private viewContainerRef: ViewContainerRef, private templateRef: TemplateRef<any>,
    private accountService: AccountService ) {
      this.accountService.currentUser$.pipe(take(1)).subscribe({
        next: user => {
          if(user) this.user = user
        }
      })
     }
  ngOnInit(): void {
    if(this.user.roles.some(r=> this.appHasRole.includes(r))){  //jeśli niektóre z ról użytkowników pasują do czegoś wewnątrz 
       this.viewContainerRef.createEmbeddedView(this.templateRef);  //tej tablicy,która zawiera role - to wyświetlimy jej zawartość
    }else{ //user nie ma wymaganej roli 2. działa podobnie jak *ngif
      this.viewContainerRef.clear();  //jeśli czyścimy kontener widoku to usuwamy ten element z dom - link admin zostanie usunięty z dom
      
    }
  }

}
