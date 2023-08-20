import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ApiService } from 'src/app/services/api.service';
import { UtilService } from 'src/app/services/util.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-global-search',
  templateUrl: './global-search.component.html',
  styleUrls: ['./global-search.component.css']
})
export class GlobalSearchComponent implements OnInit {

  search : any = {
    words: [
      {
        text: '',
        inOrder :false,
        matchExactly : false,
        DistanceWords : 0
      },
    ],
  };

  public filtersActive: boolean = false;
  showDropdown: boolean = false;
  searchText: string = '';

  obListOfParsha: any = [];
  ListOfTopices: Array<any> = [];
  ListOfTypes: Array<any> = [];
  ListOfYears: Array<any> = [];

  constructor(private api: ApiService,private util: UtilService, private router: Router) { }
  
  ngOnInit(): void {
    this.loadTypes();
    this.loadTopic();
    this.loadParshas();
    this.loadYears();
  }

  loadTypes(){
   
    this.api.GetMaamarTypes(success => {
    
      this.ListOfTypes = success;

     }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadTopic(){
   
    this.api.TopicesList({ Page: 1, ItemsPerPage: 200, sortBy: 'Name',}, success => {
   
    this.ListOfTopices = success.list;
    this.ListOfTopices.forEach(element => {
      element.color = 'btn  m-2'
    });
    }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadParshas(){
   this.api.getParshas(success => {
    //this.ListOfParsha = success

    success.forEach(element => {
 
      this.obListOfParsha.push({
            name: element,
            color: 'btn  m-2'
      }) });
     }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  loadYears(){
   

    this.api.getAllYears(success => {
    this.ListOfYears = success

     }, error => {
      console.log('error', error)
      this.util.openDeleteSource(error.messages)
    })
  }

  clickOnTop(e) {

    //console.log('clickOnTop', e);
    //e.stopPropagation();

    this.filtersActive = false;
    this.showDropdown = false;

    if(this.router.url.startsWith('/search-results')) {

      this.filtersActive = true;
      this.showDropdown = true;

    } else {

      var resultSet = this.util.GetSheardData('solrSearch');
      if(resultSet.hasOwnProperty('response')) {
        
        this.router.navigate(['search-results', {index: new Date().getTime()}])
        //console.log('click again');
        e.target.click();
      }
    }
  }

  toggleFilters(e) {

    //console.log('toggleFilters', e);
    e.stopPropagation();

    this.filtersActive = !this.filtersActive;
    this.showDropdown = this.filtersActive;
  }

  clearFilter(e) {

    //console.log('clearFilter', e);
    e.stopPropagation();

    this.filtersActive = false;
    this.showDropdown = false;
    this.searchText = '';
    this.clear();

    this.util.GetAndDeleteSheardData('solrSearch');

    if(this.router.url.startsWith('/search-results')) {
      this.util.navigate('/maamarim');
    }
  }

  cancel() {
    this.filtersActive = false;
    this.showDropdown = false;
  }

  clear(){
    this.search  = {
      words: [
        {
          text: '',
          inOrder :false,
          matchExactly : false,
          DistanceWords : 0
        },
      ],
    };

    this.ListOfTypes.forEach(x => { x.selected = false; });
    this.ListOfTopices.forEach(x => { x.selected = false; });
    this.obListOfParsha.forEach(x => { x.selected = false; });
  }

  newWord() {

    this.search.words.push({
        text: '',
        inOrder :false,
        matchExactly : false,
        DistanceWords : 0
    });

  }

  wordsToSearch() {
    return this.search.words.filter(x => x.text.trim() != '');
  }

  newSearch(dropdown) {

    var words = this.wordsToSearch();

    if(words.length == 0) {
      return;
    }


    var filter = this.buildFQ(this.ListOfTypes, 'MaamarType', 'typeValue');
    filter += this.buildFQ(this.ListOfTopices, 'Topic', 'name');
    filter += this.buildFQ(this.obListOfParsha, 'Parsha', 'name');
    filter += this.buildFQ(this.ListOfYears, 'Year', 'name');


    //console.log('filter', filter);

      /*
    var source = this.filters.Source;
    if (source.length != 0) {
      this.filter += `fq=Source%20%3A%20(${source})&`;
    }

    var createdTime =  this.filters.createdTime;

    if (createdTime.length != 0) {
      this.filter += `fq=CreatedTime%20%3A%20(${createdTime})&`;
    }

    */


    //put words on top search box
    this.searchText = '';
    for (let i = 0; i < words.length; i++) {
      this.searchText += `${words[i].text} `;
    }
      
    var searchQuery = '';

    if(words.length == 1) {

      searchQuery += this.getSearchForOneWord(words[0]);

    } else {

      searchQuery += '{!complexphrase inOrder=true} ';
      
      for (let index = 0; index < words.length; index++) {
        
        searchQuery += this.buildSearchForWord2(words, index, 'Title', true, 10);
        searchQuery += this.buildSearchForWord2(words, index, 'Content', false, 1);
      }
    }

    var additinalParams = `&start=0&rows=10&hl=on&hl.fl=Content&hl.simple.pre=<strong%20style%3D"color%3A%20blue">&hl.simple.post=</strong>&hl.snippets=50&hl.fragsize=500`;

    var url = `${environment.solrBaseUrl}?${filter}q=${searchQuery}${additinalParams}`;
    //console.log('url', searchQuery);

    this.util.loadingStart();
    this.api.solrSearch(url, success => {

      this.util.loadingStop();
    //  console.log('solrSearch', success);

      this.util.SetSheardData('solrSearch', success)
      this.router.navigate(['search-results', {index: new Date().getTime()}])
      dropdown.hide();

    }, error => {

      this.util.loadingStop();
      console.log(error);
    });
    

  }

  hasAnyFilter(list) {
    return list.some(x => x.selected);
  }

  buildFQ(list, searchType, prop) {

    var filter = '';

    var selectedItems = list.filter(x => x.selected);

    if(selectedItems.length > 0) {

      filter += `fq=${searchType}%20%3A%20(`;

      list.filter(x => x.selected).forEach(element => {
        filter += `${element[prop]} `;
      });

      filter += ")&";
    }

    return filter;
  }

  buildSearchForWord2(words, index, field, first, boost) {

    var searchQuery = '';

    if((index +1) == words.length) 
    {
      return searchQuery;
    }

    if(!first) {
      searchQuery += ' OR ';
    }
    searchQuery += "(";

    var tilda = '~';

    const element = words[index];
      
    //
    if(words.length == 1) {

      searchQuery += `${field}:(${this.getWordForSearch2(element, 0)})^${boost}`;

    } else if((index +1) != words.length) {

      //get next
      var nextElement = words[index + 1];

      if(index != 0) {
        searchQuery += " AND ";
      }

      searchQuery += ` (`;

      searchQuery += `${field}:(${this.getWordForSearch2(element, 1)} ${this.getWordForSearch2(nextElement, 2)} ${tilda}${element.DistanceWords})^${boost}`;

      if(!element.inOrder) {

        searchQuery += ` OR ${field}:(${this.getWordForSearch2(nextElement, 1)} ${this.getWordForSearch2(element, 2)} ${tilda}${element.DistanceWords})^${boost}`;

      }

      searchQuery += `)`;
    }

    searchQuery += `)`;

    return searchQuery;
  }

  getWordForSearch2(word, index) {

    var doubleQuote = "%22";
    var star = !word.matchExactly ? '*' : '';
    var leftDQ = index <= 1 ? `${doubleQuote}` : '';
    var rightDQ = index == 0 || index == 2 ? doubleQuote : '';

    return `${leftDQ}${star}${word.text}${star}${rightDQ}`;
  }

  getSearchForOneWord(word) {
    var star = !word.matchExactly ? '*' : '';
    return `(Title:(${star}${word.text}${star})^10) OR (Content:(${star}${word.text}${star})^1)`;
  }
}
