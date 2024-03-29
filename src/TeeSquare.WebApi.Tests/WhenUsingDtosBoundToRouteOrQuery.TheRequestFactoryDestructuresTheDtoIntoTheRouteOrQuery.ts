// Auto-generated Code - Do Not Edit

export interface GetRequest<TResponse> {
  url: string;
  method: 'GET';
}
export interface OptionsRequest<TResponse> {
  url: string;
  method: 'OPTIONS';
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
export interface PatchRequest<TRequest, TResponse> {
  data: TRequest;
  url: string;
  method: 'PATCH';
}
export interface PutRequest<TRequest, TResponse> {
  data: TRequest;
  url: string;
  method: 'PUT';
}
export const toQuery = (o: {[key: string]: any}): string => {
  const q = Object.keys(o)
    .map(k => ({k, v: o[k]}))
    .filter(x => x.v !== undefined && x.v !== null)
    .map(x => Array.isArray(x.v)
      ? x.v.map(v => `${encodeURIComponent(x.k)}=${encodeURIComponent(v)}`).join('&')
      : `${encodeURIComponent(x.k)}=${encodeURIComponent(x.v)}`)
    .join('&');
  return q ? `?${q}` : '';
};
export abstract class RequestFactory {
  static GetFromRouteByIdByName({ id, name }: TestObject): GetRequest<TestObject> {
    return {
      method: 'GET',
      url: `/from-route/${id}/${name}`
    };
  }
  static GetFromQuery(obj: TestObject): GetRequest<TestObject> {
    const query = toQuery({ ...obj });
    return {
      method: 'GET',
      url: `/from-query${query}`
    };
  }
  static GetDtofromrouteorqueryFromquery(numbers: number[]): GetRequest<TestObject> {
    const query = toQuery({ numbers });
    return {
      method: 'GET',
      url: `/dtofromrouteorquery/fromquery${query}`
    };
  }
}
export interface TestObject {
  id: number;
  name: string;
}
