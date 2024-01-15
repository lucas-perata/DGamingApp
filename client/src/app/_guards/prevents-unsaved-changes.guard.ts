import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanDeactivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';
import { ConfirmService } from '../_services/confirm.service'; 

@Injectable({
  providedIn: 'root'
})
export class PreventsUnsavedChangesGuard implements CanDeactivate<MemberEditComponent> {
  constructor(private confirmService: ConfirmService) {} // Inject ConfirmService

  canDeactivate(component: MemberEditComponent): Observable<boolean> | boolean {
    if(component.editForm?.dirty) {
      return this.confirmService.confirm("Are you sure you want to continue? Any unsaved changes will be lost");
    }
    return true;
  }
}