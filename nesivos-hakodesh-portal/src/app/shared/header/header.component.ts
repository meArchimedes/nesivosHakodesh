import { Component, OnInit } from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';
import { UserSettingsComponent } from '../user-settings/user-settings.component';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  user: any = {};

  constructor(
    private modalService: BsModalService,
    private api: ApiService,
    public util: UtilService
  ) { }

  ngOnInit(): void {

    this.api.AuthUser(success => {

      this.user = success;
      this.util.setCurrentUser(success);
    //  console.log('this.user', this.user);
    }, error => {
      console.log(error);
    });

    this.api.userListOpen(success => {

    //  console.log('userListOpen', success);
      this.util.setUserList(success);
    }, error => {
      console.log(error);
    });
  }

  logout() {
    this.api.AuthLogout();
  }

  openUserSettings() {
    this.modalService.show(UserSettingsComponent, { class: "settings-modal ngDraggable" });
  }
}
