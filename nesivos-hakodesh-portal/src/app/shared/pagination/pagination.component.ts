import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {

  protected _totalItems: number = 1;
  protected _itemsPerPage: number = 25;
  protected _page: number = 1;

  totalPages: number = 1;
  pages: Array<number> = [];

  @Input() objectsText: string= 'מאמרים';
  @Output() pageChanged = new EventEmitter<any>();
  @Output() itemsPerPageChanged = new EventEmitter<any>();

  @Input()
  get page(): number {
      return this._page;
  }
  set page(v: number) {
      this._page = v;
  }

  @Input()
  get itemsPerPage(): number {
      return this._itemsPerPage;
  }
  set itemsPerPage(v: number) {
      this._itemsPerPage = v;
      this.runPages();
  }

  @Input()
  get totalCount(): number {
    return this._totalItems;
  }
  set totalCount(v: number) {
    this._totalItems = v;
    this.runPages();
  }

  constructor() { }

  ngOnInit(): void {

    this.runPages();
  }

  runPages() {

    this.totalPages = this.calculateTotalPages();
    
    this.pages = [];
    for (let index = 0; index < this.totalPages; index++) {
        this.pages.push(index + 1);
    }
  }

  noPrevious(): boolean {
    return this.page === 1;
  }

  noNext(): boolean {
    return this.page === this.totalPages;
  }

  selectPage(page: number, event?: Event) {

    if (event) {
        event.preventDefault();
    }

    if(page < 1) {
      page = 1;
    }
    if(page > this.totalPages) {
      page = this.totalPages + 0;
    }
    
    

    this.page = page;
    this.pageChanged.emit(page);
  }

  updateItemsPerPage() {

    this.page = 1;
    this.itemsPerPageChanged.emit(this.itemsPerPage);
  }

  calculateTotalPages() {

    var totalPages = this.itemsPerPage < 1 ? 1 : Math.ceil(this._totalItems / this._itemsPerPage);

    return Math.max(totalPages || 0, 1);
  }
}
