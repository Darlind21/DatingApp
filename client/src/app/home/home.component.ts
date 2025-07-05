import { Component, computed, inject, OnInit } from '@angular/core';
import { RegisterComponent } from "../register/register.component";
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RegisterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  private accountService = inject(AccountService);
  registerMode =false;

  isLoggedIn = computed(() => this.accountService.currentUser() !== null);

  registerToggle(){
    if (this.isLoggedIn()) return;

    this.registerMode = !this.registerMode
  }

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

}
