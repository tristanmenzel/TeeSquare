// Auto-generated Code - Do Not Edit

export interface FailResult<TSuccess> {
  isSuccess: false;
  message: string;
}
export type Result<TSuccess> = SuccessResult<TSuccess> | FailResult<TSuccess>;
export interface SuccessResult<TSuccess> {
  isSuccess: true;
  value: TSuccess;
}
