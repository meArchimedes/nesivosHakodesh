import { Component, OnInit } from '@angular/core';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {

  user: any = {};

  constructor(private api: ApiService, public util: UtilService) { }

  ngOnInit(): void {
  }

  login() {

    this.util.loadingStart();
    this.api.AuthLogin(this.user, success => {
      this.util.loadingStop();

      this.util.navigate('/');

    }, error => {
      console.log(error);
      this.util.loadingStop();
      this.util.showError(error.messages);
    });
  }

}
