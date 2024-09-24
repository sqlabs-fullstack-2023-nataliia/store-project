import { ApiProperty } from '@nestjs/swagger';

export class CreateApiKeyRequestDto {
  @ApiProperty({ required: true })
  apiKey: string;
}
