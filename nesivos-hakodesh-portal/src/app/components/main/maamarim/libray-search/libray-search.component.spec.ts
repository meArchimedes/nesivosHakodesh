import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraySearchComponent } from './libray-search.component';

describe('LibraySearchComponent', () => {
  let component: LibraySearchComponent;
  let fixture: ComponentFixture<LibraySearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraySearchComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraySearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
