// Auto-generated Code - Do Not Edit

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
export const toQuery = (o: {[key: string]: any}): string => {
  const q = Object.keys(o)
    .map(k => ({k, v: o[k]}))
    .filter(x => x.v !== undefined && x.v !== null)
    .map(x => `${encodeURIComponent(x.k)}=${encodeURIComponent(x.v)}`)
    .join('&');
  return q && `?${q}` || '';
};
export abstract class RequestFactory {
  static ApiOtherImplicitQueryGet(id?: number): GetRequest<number> {
    const query = toQuery({id});
    return {
      method: 'GET',
      url: `api/other/implicit-query${query}`
    };
  }
  static ApiOtherImplicitRouteByIdGet(id: number): GetRequest<number> {
    return {
      method: 'GET',
      url: `api/other/implicit-route/${id}`
    };
  }
  static ApiOtherImplicitBodyPost(data: TestDto): PostRequest<TestDto, number> {
    return {
      method: 'POST',
      data,
      url: `api/other/implicit-body`
    };
  }
  static ApiOtherDoAThingGet(when?: string): GetRequest<number> {
    const query = toQuery({when});
    return {
      method: 'GET',
      url: `api/other/do-a-thing${query}`
    };
  }
  static ApiReturnTestGet(): GetRequest<unknown> {
    return {
      method: 'GET',
      url: `api/return-test`
    };
  }
  static ApiRouteNumberOneGet(): GetRequest<string> {
    return {
      method: 'GET',
      url: `api/route-number-one`
    };
  }
  static AltApiRouteNumberTwoByIdGet(id: string): GetRequest<string> {
    return {
      method: 'GET',
      url: `alt-api/route-number-two/${id}`
    };
  }
  static GettitGet(): GetRequest<boolean> {
    return {
      method: 'GET',
      url: `gettit`
    };
  }
  static ApiGet(): GetRequest<unknown> {
    return {
      method: 'GET',
      url: `api`
    };
  }
  static ApiTestByIdGet(id: number): GetRequest<TestDto> {
    return {
      method: 'GET',
      url: `api/test/${id}`
    };
  }
  static ApiTestPost(data: TestDto): PostRequest<TestDto, number> {
    return {
      method: 'POST',
      data,
      url: `api/test`
    };
  }
  static ApiTestByIdPut(id: number, data: TestDto): PutRequest<TestDto, unknown> {
    return {
      method: 'PUT',
      data,
      url: `api/test/${id}`
    };
  }
  static ApiValuesGet(): GetRequest<string[]> {
    return {
      method: 'GET',
      url: `api/values`
    };
  }
  static ApiValuesByIdGet(id: number): GetRequest<string> {
    return {
      method: 'GET',
      url: `api/values/${id}`
    };
  }
  static ApiValuesPost(data: string): PostRequest<string, void> {
    return {
      method: 'POST',
      data,
      url: `api/values`
    };
  }
  static ApiValuesByIdPut(id: number, data: string): PutRequest<string, void> {
    return {
      method: 'PUT',
      data,
      url: `api/values/${id}`
    };
  }
  static ApiValuesByIdDelete(id: number): DeleteRequest<void> {
    return {
      method: 'DELETE',
      url: `api/values/${id}`
    };
  }
}
export interface TestDto {
  hello: string;
  count: number;
  createdOn: string;
}
