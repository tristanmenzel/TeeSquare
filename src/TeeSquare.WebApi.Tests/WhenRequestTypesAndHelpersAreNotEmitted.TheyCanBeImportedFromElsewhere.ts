// Auto-generated Code - Do Not Edit

import { GetRequest, PutRequest, PostRequest, DeleteRequest, toQuery } from './WhenNoControllersAreReferenced.OnlySharedTypesAreEmitted';

export abstract class RequestFactory {
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
}
export interface TestDto {
  hello: string;
  count: number;
  createdOn: string;
}
