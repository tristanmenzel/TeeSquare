import { HubConnectionBuilder } from '@microsoft/signalr';


export const configureSignalR = async <TClient, TServer>(url, client: TClient, server: TServer) => {
  const connection = new HubConnectionBuilder().withUrl(url).build();

  await connection.start();

  for (const key in client) {
    if (!client.hasOwnProperty(key)) {
      continue;
    }
    const func = client[key];
    if (typeof (func) !== 'function') {
      continue;
    }
    // tslint:disable-next-line:only-arrow-functions
    connection.on(key, function() {
      func.apply(null, arguments);
    });
  }

  for (const key in server) {
    if (!server.hasOwnProperty(key)) {
      continue;
    }
    // tslint:disable-next-line:only-arrow-functions
    server[key] = function() {
      connection.invoke(key, ...arguments);
    } as any;
  }

  return connection;
};
