import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-search-results',
  templateUrl: './search-results.component.html',
  styleUrls: ['./search-results.component.css']
})
export class SearchResultsComponent implements OnInit {

  resultSet: any = {};
  search: any = {
    Page: 1,
    ItemsPerPage: 10
  };
  totalCount: number = 0;

  constructor(private util: UtilService, private route: ActivatedRoute, private api: ApiService) {

    route.params.subscribe(val => {
      this.loadresults();
    });
  }

  ngOnInit(): void {

    //console.log('ngOnInit');
  }

  loadresults() {

    var resultSet = this.util.GetSheardData('solrSearch');
    //console.log('resultSet', resultSet);

    if(resultSet.hasOwnProperty('response')) {

      this.resultSet = resultSet;
      this.totalCount = resultSet.response.numFound;
      this.search.ItemsPerPage = resultSet.responseHeader.params.rows;
      this.search.Page = (resultSet.responseHeader.params.start / this.search.ItemsPerPage) + 1;
      //console.log('this.search', this.search);

    } else {
      this.util.navigate('maamarim');
    }
  }

  pageChanged(e) {
    if(e != this.search.Page) {
      this.search.Page = e;
      //console.log('pageChanged', this.search.Page);
      this.SolrSearch();
    }
  }
  itemsPerPageChanged(e) {
    this.search.Page = 1;
    this.search.ItemsPerPage = Math.max(1, e);
    //console.log('itemsPerPageChanged', this.search.ItemsPerPage);
    this.SolrSearch();
  }

  SolrSearch() {

    
    var filters = "";
    
    for (var [key, value] of Object.entries(this.resultSet.responseHeader.params)) {
      //console.log(`${key}: ${value}`);

      if(Array.isArray(value)) {

        value.forEach(v => {
          filters += `${key}=${v}&`;
        });

      } else {

        if(key == 'start') {
          value = (this.search.Page - 1) * this.search.ItemsPerPage;
        }
        if(key == 'rows') {
          value = this.search.ItemsPerPage;
        }
  
        filters += `${key}=${value}&`;

      }
    }
    
    //console.log('SolrSearch', filters);

    var url = `${environment.solrBaseUrl}?${filters}`;
    //console.log('url', searchQuery);

    this.util.loadingStart();
    this.api.solrSearch(url, resultSet => {

      this.util.loadingStop();
     // console.log('solrSearch', resultSet);

      this.util.SetSheardData('solrSearch', resultSet);
      this.resultSet = resultSet;
      this.totalCount = resultSet.response.numFound;

    }, error => {

      this.util.loadingStop();
      console.log(error);
    });
  }

  maamarDetail(SearchResult){

    this.util.navigateNewWindow('maamarim/' + SearchResult.internalId[0]);
    //this.util.navigate('maamarim/' + SearchResult.internalId[0] + '?BackToSearch=true')
  }
}
