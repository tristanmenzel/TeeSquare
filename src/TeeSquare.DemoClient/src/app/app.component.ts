import { Component, inject } from '@angular/core';
import { NgForm } from '@angular/forms';
import { HubService } from './services/hub.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(private hubService: HubService) {
    this.hubService.messages.subscribe(m => this.allMessages.push(m));
  }

  allMessages: [string, string][] = [];

  currentUser: string;
  message: string;

  submit(form: NgForm) {
    if (form.valid) {
      this.allMessages.push([this.currentUser, this.message]);
      this.hubService.server.SendMessage(this.currentUser, this.message);
    }
  }
}
