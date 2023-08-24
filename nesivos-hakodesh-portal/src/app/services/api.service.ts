import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { UtilService } from './util.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ThisReceiver } from '@angular/compiler';
import { map } from 'rxjs/operators';
import { LocalCookieService } from './cookie.service';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(
    private https: HttpClient,
    private router: Router,
    private util: UtilService,
    private localCookieService: LocalCookieService
  ) { }

  //public functions
  AuthUser(success, error) {

    var option = {
      method: 'GET',
      url: 'api/Users/self'
    };

    this.makeApiRequest(option, success, error);
  }

  AuthLogin(login, success, error) {
    var option = {
      method: 'POST',
      url: 'api/Users/login',
      data: login
    };

    this.makeApiRequest(option, scs => {

      this.localCookieService.Save(scs);
      success();


    }, error);
  }
  AuthLogout() {
    this.localCookieService.Remove();
    this.router.navigate(['login']);
  }


  //Maamarim
  maamarimList(search, success, error) {

    var option = {
      method: 'POST',
      url: 'api/Maamarim/search',
      data: search
    };

    this.makeApiRequest(option, success, error);
  }




  maamarimFolders(success, error) {

    var option = {
      method: 'GET',
      url: 'api/Maamarim/folders'
    };

    this.makeApiRequest(option, success, error);
  }

  AddMaamar(data, success, error){
    var option ={
      method: 'POST',
      url: 'api/Maamarim',
      data: data,
    };
    this.makeApiRequest(option, success, error);
  }

  Getmaamar(id, success , error){
    var option = {
      method:'GET',
      url: 'api/Maamarim/' + id,
    };
    this.makeApiRequest(option, success, error);
  }

  UpdateMaamar(data, success, error){

    var option = {
      method: 'PUT',
      url: 'api/Maamarim' ,
      data: data
    };
    this.makeApiRequest(option, success, error);
  }
  DeleteMaamar(id, success, error){
    var option = {
      method: 'DELETE',
      url: 'api/Maamarim/' + id ,

    };
    this.makeApiRequest(option, success, error);
  }

  GetMaamarTypes(success , error){
    var option = {
      method:'GET',
      url: 'api/Maamarim/types',
    };
    this.makeApiRequest(option, success, error);
  }


  GetMaamarStatus(success , error){
    var option = {
      method:'GET',
      url: 'api/Maamarim/status',
    };
    this.makeApiRequest(option, success, error);
  }

  SearchLibrary(term){
console.log('term', term)
    var url = environment.apiBaseUrl + 'api/Library/search';
    var httpOptions = this.getHttpOptions(false);

    var search = term


    return this.http.post(url, search, httpOptions)
    .pipe(map(response => response['data']['list']));
  }

  LibraryDetails(search, success, error) {

    var option = {
      method: 'POST',
      url: 'api/Library/details',
      data: search
    };

    this.makeApiRequest(option, success, error);
  }

  LibraryList(search, success, error) {

    var option = {
      method: 'POST',
      url: 'api/Library/search',
      data: search
    };

    this.makeApiRequest(option, success, error);
  }

  maamarLibraryLinks(id, success, error) {

    var option = {
      method: 'GET',
      url: 'api/Library/' + id,

    };

    this.makeApiRequest(option, success, error);
  }
  LibraryDetailsForScroll(search, success, error) {

    var option = {
      method: 'POST',
      url: 'api/Library/Scrolldetails',
      data: search
    };

    this.makeApiRequest(option, success, error);
  }

  DeleteMaamarLibraryLink(id, success, error){
    var option = {
      method: 'DELETE',
      url: 'api/Library/' + id ,

    };
    this.makeApiRequest(option, success, error);
  }

  GetCategoryFilters(success, error){
    var option = {
      method: 'GET',
      url: 'api/Library/Categoryfilters',

    };

    this.makeApiRequest(option, success, error);
  }

  GetTypeFilters(search, success, error){
    var option = {
      method: 'POST',
      url: 'api/Library/Typefilters',
      data: search
    };

    this.makeApiRequest(option, success, error);
  }

  GetSectionFilters(search, success, error){
    var option = {
      method: 'POST',
      url: 'api/Library/Sectionfilters',
      data: search
    };

    this.makeApiRequest(option, success, error);
  }

  GetChepterFilters(search, success, error){
    var option = {
      method: 'POST',
      url: 'api/Library/Chopterfilters',
      data: search
    };

    this.makeApiRequest(option, success, error);
  }

  solrSearch(url, success, error) {

    var option = {
      method: 'GET',
      url: url,
      externalUrl: true
    };
    this.makeApiRequest(option, success, error);

  }


//Topics

TopicesList(search, success, error) {

  var option = {
    method: 'POST',
    url: 'api/Topics/search',
    data: search
  };

  this.makeApiRequest(option, success, error);
}

// Topices
TopiceList(search, success, error) {

  var option = {
    method: 'POST',
    url: 'api/Topics/search',
    data: search
  };

  this.makeApiRequest(option, success, error);
}

GetTopice(id, success , error){
  var option = {
    method:'GET',
    url: 'api/Topics/' + id,
  };
  this.makeApiRequest(option, success, error);
}

AddTopice(Torah, success, error){
  var option = {
    method: 'POST',
    url: 'api/Topics',
    data: Torah
  };
  this.makeApiRequest(option, success, error);
}

UpdateTopice(torah, success, error){
  var option = {
    method: 'PUT',
    url: 'api/Topics',
    data: torah
  };
  this.makeApiRequest(option, success, error);
}

DeleteTopice(id, success, error){
  var option = {
    method: 'DELETE',
    url: 'api/Topics/' + id ,

  };
  this.makeApiRequest(option, success, error);
}
GetSeforimTopices(success , error){
  var option = {
    method: 'GET',
    url: 'api/Topics/Seforim' ,

  };
  this.makeApiRequest(option, success, error);
}

GetSeforimList(success , error){
  var option = {
    method: 'GET',
    url: 'api/Sefurim/list'
  };
  this.makeApiRequest(option, success, error);
}

GetTopicesForDropDown(success , error){
  var option = {
    method: 'GET',
    url: 'api/Topics/List' ,

  };
  this.makeApiRequest(option, success, error);
}

//Source
SourceList(search, success, error) {

  var option = {
    method: 'POST',
    url: 'api/Sources/search',
    data: search
  };

  this.makeApiRequest(option, success, error);
}

SearchSource(term){
  var url = environment.apiBaseUrl + 'api/Sources/search';
  var httpOptions = this.getHttpOptions(false);

  var search = {
    SearchTerm: term,
    Page: 1,
    ItemsPerPage: 10,
    SortBy: 'FirstName',
    SortDirection: 'Ascending'
  };
  return this.http.post(url, search, httpOptions)
  .pipe(map(response => response['data'] ['list']));
}

AddSource(source, success, error){
  var option = {
    method: 'POST',
    url: 'api/Sources',
    data: source
  };
  this.makeApiRequest(option, success, error);
}

UpdateSource(source, success, error){
  var option = {
    method: 'PUT',
    url: 'api/Sources',
    data: source
  };
  this.makeApiRequest(option, success, error);
}

DeleteSource(id, success, error){
  var option = {
    method: 'DELETE',
    url: 'api/Sources/' + id ,

  };
  this.makeApiRequest(option, success, error);
}



// Sefurim
SefurimList(search, success, error) {

  var option = {
    method: 'POST',
    url: 'api/Sefurim/search',
    data: search
  };

  this.makeApiRequest(option, success, error);
}

GetSafer(id, success , error){
  var option = {
    method:'GET',
    url: 'api/Sefurim/' + id,
  };
  this.makeApiRequest(option, success, error);
}

UpdateSefur(Sefur, success, error){
  var option = {
    method: 'PUT',
    url: 'api/Sefurim',
    data: Sefur
  };
  this.makeApiRequest(option, success, error);
}

AddSefur(Sefur, success, error){
  var option = {
    method: 'POST',
    url: 'api/Sefurim',
    data: Sefur
  };
  this.makeApiRequest(option, success, error);
}

DeleteSefur(id, success, error){
  var option = {
    method: 'DELETE',
    url: 'api/Sefurim/' + id ,

  };
  this.makeApiRequest(option, success, error);
}


// parsha
getParshas(success, error){
  var option = {
    method: 'GET',
    url: 'api/Parsha',

  };

  this.makeApiRequest(option, success, error);
}

getParshas2(success, error){
  var option = {
    method: 'GET',
    url: 'api/Parsha/withBooks',

  };

  this.makeApiRequest(option, success, error);
}

getAllYears(success, error){
  var option = {
    method: 'GET',
    url: 'api/Parsha/Years',
  };

  this.makeApiRequest(option, success, error);
}

// Torahs
TorahsList(search, success, error) {

  var option = {
    method: 'POST',
    url: 'api/Torahs/search',
    data: search
  };

  this.makeApiRequest(option, success, error);
}

GetTorah(id, success , error){
  var option = {
    method:'GET',
    url: 'api/Torahs/' + id,
  };
  this.makeApiRequest(option, success, error);
}

AddTorah(Torah, success, error){
  var option = {
    method: 'POST',
    url: 'api/Torahs',
    data: Torah
  };
  this.makeApiRequest(option, success, error);
}

UpdateTorah(torah, success, error){
  var option = {
    method: 'PUT',
    url: 'api/Torahs',
    data: torah
  };
  this.makeApiRequest(option, success, error);
}

DeleteTorah(id, success, error){
  var option = {
    method: 'DELETE',
    url: 'api/Torahs/' + id ,

  };
  this.makeApiRequest(option, success, error);
}

// MaamarTorahLink
AddMaamarTorahLink(MaamarTorah, success, error){
  var option = {
    method: 'POST',
    url: 'api/MaamarTorahLink',
    data: MaamarTorah
  };
  this.makeApiRequest(option, success, error);
}

GetMaamarTorahLink(id, success, error){
  var option = {
    method: 'GET',
    url: 'api/MaamarTorahLink/' + id,

  };
  this.makeApiRequest(option, success, error);
}

DeleteMaamarTorahLink(id, success, error){
  var option = {
    method: 'DELETE',
    url: 'api/MaamarTorahLink/' + id ,

  };
  this.makeApiRequest(option, success, error);
}

// Category
GetCategoryList(search, success, error) {

  var option = {
    method: 'POST',
    url: 'api/Categories/search',
    data: search
  };

  this.makeApiRequest(option, success, error);
}

GetCategory(id, success , error){
  var option = {
    method:'GET',
    url: 'api/Categories/' + id,
  };
  this.makeApiRequest(option, success, error);
}

AddCategory(data, success, error){
  var option = {
    method: 'POST',
    url: 'api/Categories',
    data: data
  };
  this.makeApiRequest(option, success, error);
}

UpdateCategory(data, success, error){
  var option = {
    method: 'PUT',
    url: 'api/Categories',
    data: data
  };
  this.makeApiRequest(option, success, error);
}

DeleteCategory(id, success, error){
  var option = {
    method: 'DELETE',
    url: 'api/Categories/' + id ,

  };
  this.makeApiRequest(option, success, error);
}

//Solr
AddRecordsToSolr(success, error){

  var option = {
    method: 'GET',
    url: 'api/test/solr',

  };

  this.makeApiRequest(option, success, error);
}


//Users


userList(search, success, error) {

  var option = {
    method: 'POST',
    url: 'api/Users/search',
    data: search
  };

  this.makeApiRequest(option, success, error);
}

userListOpen(success, error) {

  var option = {
    method: 'GET',
    url: 'api/Users/openUsers'
  };

  this.makeApiRequest(option, success, error);
}

Getuser(id, success , error){
  var option = {
    method:'GET',
    url: 'api/Users/' + id,
  };
  this.makeApiRequest(option, success, error);
}

Adduser(user, success, error){
  var option = {
    method: 'POST',
    url: 'api/Users',
    data: user
  };
  this.makeApiRequest(option, success, error);
}

Updateuser(user, success, error){
  var option = {
    method: 'PUT',
    url: 'api/Users',
    data: user
  };
  this.makeApiRequest(option, success, error);
}

Deleteuser(id, success, error){
  var option = {
    method: 'DELETE',
    url: 'api/Users/' + id ,

  };
  this.makeApiRequest(option, success, error);
}

  UploadFile(type, id, subType, file, successCallback, errorCallback) {

    var url =  environment.apiBaseUrl + `api/Files/upload/${type}/${id}/${subType}` ;
    var formData = new FormData();

    formData.append(file.name, file);
    this.http.post(url, formData, this.getHttpOptions(true)).subscribe(data => successCallback(data['data']), error => errorCallback(error.error));

  }

  ReadFile(filePath, type) {

    var url = environment.apiBaseUrl + 'api/Files/read/' + type;

    var httpOptions: any = {
      headers: new HttpHeaders({
        'Authorization':  'Bearer ' + this.localCookieService.Get(),
        Accept:'application/pdf'
      }),
      responseType: "arraybuffer",
      observe: 'response'
    };

    return this.http.post(url, {SearchTerm: filePath}, httpOptions).toPromise();
  }

  //private functions
  private makeApiRequest(option, success, errorCallback): Subscription {
    var $this = this;

    return this.apiRequest(option, success, function(error) {

      console.log(error);
      $this.util.loadingStop();
      if (error.status == 401) {
        $this.router.navigate(['/login']);

      } else if (error.status == 403) {

        $this.util.openDeleteSource("אין לך הרשאה לבצע פעולה זו");

      } else{
        errorCallback(error.error);
      }
    });
  }

  private apiRequest(option, success, errorCallback): Subscription {


    var url;
    var httpOptions;
    var responseDataFieldName = "data";

    if(option.externalUrl) {

      url = option.url;
      httpOptions = {};
      //responseDataFieldName = 'response';
      responseDataFieldName = '';

    } else {

      url = environment.apiBaseUrl + option.url;
      httpOptions = this.getHttpOptions(option.isFile);

    }

    if (option.method == 'GET') {
      if(option.data) {
        url = this.getUrlWithParams(url, option.data);
      }

      return this.http
        .get(url, httpOptions)
        .subscribe(
          data => {

            if(responseDataFieldName != '') {
              success(data[responseDataFieldName]);
            } else {
              success(data);
            }

          },
          error => errorCallback(error)
        );
    } else if (option.method == 'POST') {
      return this.http
        .post(url, option.data, httpOptions)
        .subscribe(
          data => success(data[responseDataFieldName]),
          error => errorCallback(error)
        );
    } else if (option.method == 'PUT') {

      return this.http
        .put(url, option.data, httpOptions)
        .subscribe(
          data => success(data[responseDataFieldName]),
          error => errorCallback(error)
        );
    } else if (option.method == 'DELETE') {
      if(option.data) {
        url = this.getUrlWithParams(url, option.data);
      }

      return this.http
        .delete(url, httpOptions)
        .subscribe(
          data => success(data[responseDataFieldName]),
          error => errorCallback(error)
        );
    }
  }

  private getHttpOptions(isFile: boolean = false) {

    var headers = {
      Authorization: 'Bearer ' + this.localCookieService.Get()
    };

    if(!isFile) {
      headers['Content-Type'] = 'application/json';
    }

    var httpOptions: any = {
      headers: new HttpHeaders(headers)
    };

    return httpOptions;
  }

  private getUrlWithParams(url: string, data: any): string {
    let params = new URLSearchParams();
    for(let key in data) {
      if(Array.isArray(data[key])) {
        for(let value in data[key]) {
          params.append(key, data[key][value]);
        }
      } else if(data[key] != '') {
        params.set(key, data[key]);
      }
    }
    return url + "?" + params.toString();
  }
}
