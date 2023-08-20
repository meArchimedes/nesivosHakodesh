import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MergeMaamarPopUpComponent } from './merge-maamar-pop-up.component';

describe('MergeMaamarPopUpComponent', () => {
  let component: MergeMaamarPopUpComponent;
  let fixture: ComponentFixture<MergeMaamarPopUpComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MergeMaamarPopUpComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MergeMaamarPopUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
