export interface GetRequest<TResponse> {
  url: string;
}
export interface DeleteRequest<TResponse> {
  url: string;
}
export interface PostRequest<TRequest, TResponse> {
  data: TRequest;
  url: string;
}
export interface PutRequest<TRequest, TResponse> {
  data: TRequest;
  url: string;
}
export abstract class RequestFactory {
  static toQuery(o: {[key:string]: any}): string {
    let q = Object.keys(o)
      .map(k=>({k, v: o[k]}))
      .filter(x=>x.v !== undefined && x.v===null)
      .map(x=>`${encodeURIComponent(x.k)}=${encodeURIComponent(x.v)}`)
      .join('&');
    return q && `?${q}` || '';
  }
  static GetApiOtherDoAThing(when?: string): GetRequest<number> {
    let query = RequestFactory.toQuery({when});
    return {
      url: `api/other/do-a-thing${query}`
    };
  }
  static GetApiRouteNumberOne(): GetRequest<string> {
    return {
      url: `api/route-number-one`
    };
  }
  static GetAltApiRouteNumberTwo(id: string): GetRequest<string> {
    return {
      url: `alt-api/route-number-two`
    };
  }
  static GetGettit(): GetRequest<boolean> {
    return {
      url: `gettit`
    };
  }
  static GetApi(): GetRequest<any> {
    return {
      url: `api`
    };
  }
  static GetApiValues(): GetRequest<string[]> {
    return {
      url: `api/values`
    };
  }
  static GetApiValuesById(id: number): GetRequest<string> {
    return {
      url: `api/values/${id}`
    };
  }
  static PostApiValues(data: string): PostRequest<string, void> {
    return {
      data,
      url: `api/values`
    };
  }
  static PutApiValuesById(id: number, data: string): PutRequest<string, void> {
    return {
      data,
      url: `api/values/${id}`
    };
  }
  static DeleteApiValuesById(id: number): DeleteRequest<void> {
    return {
      url: `api/values/${id}`
    };
  }
}
