import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie';
import { environment } from 'src/environments/environment';


@Injectable({
    providedIn: 'root'
})
export class LocalCookieService {

    constructor(
        private cookie: CookieService
    ) { }

    
    Save(scs) {

        var now = new Date();
        now.setDate(now.getDate() + 30);
        this.cookie.put(environment.cookie, scs, {expires: now});
  
        /*
        var obj = {
            expires: now,
            token: scs
        };
        this.localStorageService.set(environment.cookie, JSON.stringify(obj));
        */
    }

    Get() {

        var token = this.cookie.get(environment.cookie);
        
        if(token) {
            return token;
        }

        /*
        var token2 = this.localStorageService.get(environment.cookie);

        if(token2) {

            var tokenObj = JSON.parse(token2 + '');
    
            var now = Date.now();
            if(tokenObj.expires < now) {
                this.localStorageService.remove(environment.cookie);
                return '';
            }
    
            return tokenObj.token;
        }
        */

        return '';
    }

    Remove() {

        this.cookie.remove(environment.cookie);
        //this.localStorageService.remove(environment.cookie);
    }
}
