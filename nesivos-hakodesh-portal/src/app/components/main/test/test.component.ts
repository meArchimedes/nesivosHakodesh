import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }


    // search : any = {
  //   words: [
  //     {
  //       text: ' הַלְלוּיָהּ הַלְלוּ',
  //      inOrder :false,
  //      matchExactly : true,
  //      DistanceWords : 100
  //    },
  //     {
  //       text: 'אַשְׁרֵי  הַלְלוּיָהּ ',
  //       inOrder: false,
  //       matchExactly : true,
  //       DistanceWords : 1
  //     }

  //   ]

  // };
 // newWord : [];
 // newWord2: string = '';
  search : any = {
    words: [
      {
      text: 'טשיינע',
       inOrder :false,
       matchExactly : true,
       DistanceWords : 0
    },
    {
      text: 'מצב',
       inOrder :false,
       matchExactly : true,
       DistanceWords : 0
    },
  ],

  };

  filters : any = {
    topic : [],
    Source: [],
    mammarType: [],
    parsha : [],
    createdTime : [],


};

filter: string = '';

  createWord(){


   }

  newWord() {

    this.search.words.push({
        text: '',
       inOrder :false,
       matchExactly : true,
       DistanceWords : 0
    });

  }

  newSearch() {

    var source = this.filters.Source;
    if (source.length != 0) {
      this.filter += `fq=Source%20%3A%20(${source})&`;
    }

    var topic =  this.filters.topic;

    if (topic.length != 0) {
      this.filter += `fq=Topic%20%3A%20(${topic})&`;
    }

    var mammarType =  this.filters.mammarType;
    if (mammarType.length != 0) {
      this.filter += `fq=MaamarType%20%3A%20(${mammarType})&`;
    }

    var parsha =  this.filters.parsha;

    if (parsha.length != 0) {
      this.filter += `fq=Parsha%20%3A%20(${parsha})&`;
    }

    var createdTime =  this.filters.createdTime;

    if (createdTime.length != 0) {
      this.filter += `fq=CreatedTime%20%3A%20(${createdTime})&`;
    }

    this.filters.Source = [];
// fq=Type%20%3A%20(Maamarim%20OR%20Torahs)&
// (${topic})

    var q = "";
    var a = "";
    var b = "";
    if (this.search.matchExactly ) {
      a = '';
      b = '"';
;

    };


    //

    var searchQuery = '{!complexphrase inOrder=true} ';

    for (let index = 0; index < this.search.words.length; index++) {

      searchQuery += this.buildSearchForWord2(index, 'Title', true, 10);
      searchQuery += this.buildSearchForWord2(index, 'Content', false, 1);

    }




    //
    //{!complexphrase inOrder=false}"abc xyz"~6
    //q += "%28{!complexphrase inOrder=true}%22";
   // q += '(%22';
   // b = '%28';
    //a = '%29';

// for english
    // this.search.words.forEach(word => {

    //  var p = !word.matchExactly ? '*' : '%22';
    //   var k ='~'; // ~
    //   b = !word.matchExactly ? '*' : '%22';
    //   a = '%20('
    //  var d = '(';

    //   q += `${d}${p}${word.text}${b} ${k}${word.DistanceWords})`;
    // });

    //for yiddish
   // q += '{!complexphrase inOrder=true}';

    this.search.words.forEach(word => {
//word.text = this.newWord1;

      var p = !word.matchExactly ? '*' : '"';
       var k ='~'; // ~
       b = !word.matchExactly ? '*' : '%22';
       a = '%20'
      var d = '(';

       q += `${d}${p}${word.DistanceWords} ${k} ${p}${a}${word.text})`;

     });

   // q += "%22%7E2";

    //genrate solr url
    var url = `https://localhost:8983/solr/myproducts/msssearch?${this.filter}q=${searchQuery}`;
    // &defType=dismax&qf=text%20contact
    //
    //var url = `https://localhost:8983/solr/myproducts/select?defType=dismax&q=${searchQuery}&qf=Content%5E1%20text%5E2&start=0`;
    window.open(url, "_blank");


  }

  getWordForSearch(word, index) {

    var doubleQuote = "%22";
    var star = !word.matchExactly ? '*' : '';
    var leftDQ = index <= 1 ? doubleQuote : '';
    var rightDQ = index == 0 || index == 2 ? doubleQuote : '';

    return `${leftDQ}${star}${word.text}${star}${rightDQ}`;
  }

  buildSearchForWord(index) {

    var searchQuery = '';
    var tilda = '~';

    const element = this.search.words[index];

    //
    if(this.search.words.length == 1) {

      searchQuery += `(${this.getWordForSearch(element, 0)})`;

    } else if((index +1) != this.search.words.length) {

      //get next
      var nextElement = this.search.words[index + 1];

      if(index != 0) {
        searchQuery += " AND ";
      }

      searchQuery += ` (`;

      searchQuery += `(${this.getWordForSearch(element, 1)} ${this.getWordForSearch(nextElement, 2)} ${tilda}${element.DistanceWords})`;

      if(!element.inOrder) {

        searchQuery += ` OR (${this.getWordForSearch(nextElement, 1)} ${this.getWordForSearch(element, 2)} ${tilda}${element.DistanceWords})`;

      }

      searchQuery += `)`;
    }

    return searchQuery;
  }

  buildSearchForWord2(index, field, first, boost) {

    var searchQuery = '';

    if((index +1) == this.search.words.length)
    {
      return searchQuery;
    }

    if(!first) {
      searchQuery += ' OR ';
    }
    searchQuery += "(";

    var tilda = '~';

    const element = this.search.words[index];

    //
    if(this.search.words.length == 1) {

      searchQuery += `${field}:(${this.getWordForSearch2(element, 0)})^${boost}`;

    } else if((index +1) != this.search.words.length) {

      //get next
      var nextElement = this.search.words[index + 1];

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

}
