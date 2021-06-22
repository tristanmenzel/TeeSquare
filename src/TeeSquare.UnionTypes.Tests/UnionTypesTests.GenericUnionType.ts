// Auto-generated Code - Do Not Edit

export interface FailResult<TSuccess> {
  message: string;
  isSuccess: false;
}
export type Result<TSuccess> = SuccessResult<TSuccess> | FailResult<TSuccess>;
export interface SuccessResult<TSuccess> {
  value: TSuccess;
  isSuccess: true;
}
