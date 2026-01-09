export interface Service {
  serviceId: number;
  name: string;
  description: string;
  serviceCharge: number;
  serviceCategoryId: number;
  categoryName?: string;
}
