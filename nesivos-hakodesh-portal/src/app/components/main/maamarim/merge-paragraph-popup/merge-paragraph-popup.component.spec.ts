import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MergeParagraphPopupComponent } from './merge-paragraph-popup.component';

describe('MergeParagraphPopupComponent', () => {
  let component: MergeParagraphPopupComponent;
  let fixture: ComponentFixture<MergeParagraphPopupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MergeParagraphPopupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MergeParagraphPopupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
