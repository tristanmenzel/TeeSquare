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
  static GetDefaultrouteIndex(): GetRequest<number> {
    return {
      method: 'GET',
      url: `defaultroute/index`
    };
  }
  static GetDefaultrouteGetnumById(id: number): GetRequest<number> {
    return {
      method: 'GET',
      url: `defaultroute/getnum/${id}`
    };
  }
  static PostFormvaluePostsomevalues(name: string, specialFile: File): PostRequest<FormData, number> {
    const data = new FormData();
    data.append('name', name);
    data.append('specialFile', specialFile);
    return {
      method: 'POST',
      data,
      url: `formvalue/postsomevalues`
    };
  }
  static GetApiOtherImplicitQuery(id?: number): GetRequest<number> {
    const query = toQuery({id});
    return {
      method: 'GET',
      url: `api/other/implicit-query${query}`
    };
  }
  static GetApiOtherImplicitRouteById(id: number): GetRequest<number> {
    return {
      method: 'GET',
      url: `api/other/implicit-route/${id}`
    };
  }
  static PostApiOtherImplicitBody(data: TestDto): PostRequest<TestDto, number> {
    return {
      method: 'POST',
      data,
      url: `api/other/implicit-body`
    };
  }
  static GetApiOtherDoAThing(when?: string): GetRequest<number> {
    const query = toQuery({when});
    return {
      method: 'GET',
      url: `api/other/do-a-thing${query}`
    };
  }
  static GetApiReturnTest(): GetRequest<ActionResult> {
    return {
      method: 'GET',
      url: `api/return-test`
    };
  }
  static GetApiRouteconstraintsUserById(id: number): GetRequest<string> {
    return {
      method: 'GET',
      url: `api/routeconstraints/user/${id}`
    };
  }
  static GetApiRouteconstraintsUserByName(name: string, limit?: number): GetRequest<string[]> {
    const query = toQuery({limit});
    return {
      method: 'GET',
      url: `api/routeconstraints/user/${name}${query}`
    };
  }
  static GetApiRouteconstraintsUserByNameByPageByPageSize(name: string, page: number, pageSize: number): GetRequest<string[]> {
    return {
      method: 'GET',
      url: `api/routeconstraints/user/${name}/${page}/${pageSize}`
    };
  }
  static PutApiRouteconstraintsUserByAge(age: number, data: TestDto): PutRequest<TestDto, ActionResult> {
    return {
      method: 'PUT',
      data,
      url: `api/routeconstraints/user/${age}`
    };
  }
  static GetApiRouteNumberOne(): GetRequest<string> {
    return {
      method: 'GET',
      url: `api/route-number-one`
    };
  }
  static GetAltApiRouteNumberTwoById(id: string): GetRequest<string> {
    return {
      method: 'GET',
      url: `alt-api/route-number-two/${id}`
    };
  }
  static GetGettit(): GetRequest<boolean> {
    return {
      method: 'GET',
      url: `gettit`
    };
  }
  static GetApi(): GetRequest<ActionResult> {
    return {
      method: 'GET',
      url: `api`
    };
  }
  static GetApiTestById(id: number): GetRequest<TestDto> {
    return {
      method: 'GET',
      url: `api/test/${id}`
    };
  }
  static PostApiTest(data: TestDto): PostRequest<TestDto, number> {
    return {
      method: 'POST',
      data,
      url: `api/test`
    };
  }
  static PatchApiTest(data: TestDto): PatchRequest<TestDto, number> {
    return {
      method: 'PATCH',
      data,
      url: `api/test`
    };
  }
  static OptionsApiTest(): OptionsRequest<number> {
    return {
      method: 'OPTIONS',
      url: `api/test`
    };
  }
  static PutApiTestById(id: number, data: TestDto): PutRequest<TestDto, ActionResult> {
    return {
      method: 'PUT',
      data,
      url: `api/test/${id}`
    };
  }
  static PostApiTestBySomeId(someId: number): PostRequest<null, ActionResult> {
    return {
      method: 'POST',
      data: null,
      url: `api/test/${someId}`
    };
  }
  static PatchApiTestBySomeId(someId: number): PatchRequest<null, ActionResult> {
    return {
      method: 'PATCH',
      data: null,
      url: `api/test/${someId}`
    };
  }
  static PutApiTestBySomeId(someId: number): PutRequest<null, ActionResult> {
    return {
      method: 'PUT',
      data: null,
      url: `api/test/${someId}`
    };
  }
  static GetApiValues(): GetRequest<string[]> {
    return {
      method: 'GET',
      url: `api/values`
    };
  }
  static GetApiValuesById(id: number): GetRequest<string> {
    return {
      method: 'GET',
      url: `api/values/${id}`
    };
  }
  static PostApiValues(data: string): PostRequest<string, void> {
    return {
      method: 'POST',
      data,
      url: `api/values`
    };
  }
  static PutApiValuesById(id: number, data: string): PutRequest<string, void> {
    return {
      method: 'PUT',
      data,
      url: `api/values/${id}`
    };
  }
  static DeleteApiValuesById(id: number): DeleteRequest<void> {
    return {
      method: 'DELETE',
      url: `api/values/${id}`
    };
  }
}
export interface ActionResult {
}
export interface NotUsedInApiTestDto {
  hello: string;
  count: number;
  createdOn: string;
}
export interface TestDto {
  hello: string;
  count: number;
  createdOn: string;
}
