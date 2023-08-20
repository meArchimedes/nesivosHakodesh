import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { DeleteMaamarComponent } from '../components/main/maamarim/delete-maamar/delete-maamar.component';


export interface ComponentCanDeactivate {
  canDeactivate(): boolean | Observable<boolean>;
  saveAndGoBack(successCallback): void;
}

export const CanDeactivateState = {
  defendAgainstBrowserBackButton: false,
};

@Injectable()
export class CanGoBackGuard implements CanDeactivate<ComponentCanDeactivate> {

  constructor(private modalService: BsModalService) 
  {

  }

  canDeactivate(component: ComponentCanDeactivate): boolean | Observable<boolean> | Promise<boolean> {

    return component.canDeactivate() || new Promise((resolve, reject) => {

        this.modalService.show(DeleteMaamarComponent, { 
            class: "delete-maamar-modal ngDraggable" , 
            backdrop:'static',
            initialState: {
              contact: {
                title: 'אזהרה',
                okButton: 'לשמור',
                deleteButton: 'עזוב בלי לשמור',
                Msg: '?האם אתה בטוח שאתה רוצה לעזוב מבלי לשמור את השינויים שלך',
                Del: true
              }
            }
          }).content.onClose.subscribe(result => {

           // console.log('result', result);

            //cancel
            if(result.cancel) {
              resolve(false);
            }
            //do not save
            else if (result.success) {
                resolve(true);
            } else {
                //save 
                component.saveAndGoBack((success) => {
                    resolve(success);
                });
            }
        });
    })
  }
}
