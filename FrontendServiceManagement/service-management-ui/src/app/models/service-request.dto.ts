export interface ServiceRequestDto {
  serviceRequestId: number;
  serviceName: string;
  customerName: string;
  status: string;
  priority: string;
  requestedAt: string;
  scheduledAt?: string | null;
}
