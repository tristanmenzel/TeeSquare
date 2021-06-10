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
  static PostApiOtherDoAThing(data?: string): PostRequest<string | undefined, number> {
    return {
      method: 'POST',
      data,
      url: `/api/other/do-a-thing`
    };
  }
}
