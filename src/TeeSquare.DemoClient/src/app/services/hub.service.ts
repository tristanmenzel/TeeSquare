import { Injectable } from '@angular/core';
import { HubConnection } from '@microsoft/signalr';
import { Observable, ReplaySubject } from 'rxjs';
import { ApplicationHubClient, ApplicationHubServer } from '../../codegen/api';
import { environment } from '../../environments/environment';
import { configureSignalR } from '../../hubs/application-hub';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  server: ApplicationHubServer = {
    SendMessage(user: string, message: string): void {
    }
  };
  client: ApplicationHubClient = {
    ReceiveMessage: (user: string, message: string): void => {
      this.messagesSubject.next([user, message]);
    }
  };

  hub: HubConnection;

  private messagesSubject = new ReplaySubject<[string, string]>(10);

  get messages(): Observable<[string, string]> {
    return this.messagesSubject;
  }

  constructor() {
    this.init();
  }

  async init() {
    this.hub = await configureSignalR(environment.signalRHub, this.client, this.server);

  }
}
