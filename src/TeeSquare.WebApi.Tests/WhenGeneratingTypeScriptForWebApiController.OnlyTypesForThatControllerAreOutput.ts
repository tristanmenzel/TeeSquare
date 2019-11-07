export interface GetRequest<TResponse> {
  url: string;
  method: 'GET';
}
export interface DeleteRequest<TResponse> {
  url: string;
  method: 'DELETE';
}
export interface PostRequest<TRequest, TResponse> {
  data: TRequest;
  url: string;
  method: 'POST';
}
export interface PutRequest<TRequest, TResponse> {
  data: TRequest;
  url: string;
  method: 'PUT';
}
export abstract class RequestFactory {
  static toQuery(o: {[key: string]: any}): string {
    let q = Object.keys(o)
      .map(k => ({k, v: o[k]}))
      .filter(x => x.v !== undefined && x.v !== null)
      .map(x => `${encodeURIComponent(x.k)}=${encodeURIComponent(x.v)}`)
      .join('&');
    return q && `?${q}` || '';
  }
  static GetApiRouteNumberOne(): GetRequest<string> {
    return {
      method: 'GET',
      url: `api/route-number-one`
    };
  }
  static GetAltApiRouteNumberTwo(id: string): GetRequest<string> {
    return {
      method: 'GET',
      url: `alt-api/route-number-two`
    };
  }
  static GetGettit(): GetRequest<boolean> {
    return {
      method: 'GET',
      url: `gettit`
    };
  }
  static GetApi(): GetRequest<any> {
    return {
      method: 'GET',
      url: `api`
    };
  }
}
