import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LocalCookieService } from '../services/cookie.service';

@Injectable({
	providedIn: 'root'
})
export class AuthenticatedUserGuard implements CanActivate {

	constructor(public router: Router, private cookie: LocalCookieService) { }

	canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean  {

		var co = this.cookie.Get();
		if (!co) {
			this.router.navigate(['login']);
			return false;
		}

		return true;
	}

}
