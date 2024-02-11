import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnterLinkUsernameComponent } from './enter-link-username.component';

describe('EnterLinkUsernameComponent', () => {
  let component: EnterLinkUsernameComponent;
  let fixture: ComponentFixture<EnterLinkUsernameComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EnterLinkUsernameComponent]
    });
    fixture = TestBed.createComponent(EnterLinkUsernameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
