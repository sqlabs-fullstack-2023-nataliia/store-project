import { ApiProperty } from '@nestjs/swagger';

export class ValidateApiKeyRequestDto {
  @ApiProperty({ required: true })
  apiKey: string;
}
