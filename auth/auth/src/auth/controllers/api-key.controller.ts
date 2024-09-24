import { Controller, Post, Body, UnauthorizedException } from '@nestjs/common';
import { ApiKeyService } from '../services/api-key.service';
import { ApiBody, ApiTags } from '@nestjs/swagger';
import { CreateApiKeyRequestDto } from '../dto/create-api-key-request.dto';
import { ValidateApiKeyRequestDto } from '../dto/validate-api-key-request.dto';

@ApiTags('API Keys')
@Controller('api-keys')
export class ApiKeyController {
  constructor(private readonly apiKeyService: ApiKeyService) {}

  @Post('create')
  async createApiKey(@Body() createApiKeyRequestDto: CreateApiKeyRequestDto) {
    if (!createApiKeyRequestDto.apiKey) {
      throw new UnauthorizedException('API Key is required');
    }
    return this.apiKeyService.createApiKey(createApiKeyRequestDto.apiKey);
  }

  @Post('validate')
  async validateApiKey(
    @Body() validateApiKeyRequestDto: ValidateApiKeyRequestDto,
  ) {
    if (!validateApiKeyRequestDto.apiKey) {
      throw new UnauthorizedException('API Key is required');
    }

    // Validate the API key and return the JWT token
    return this.apiKeyService.validateApiKeyAndGenerateToken(
      validateApiKeyRequestDto.apiKey,
    );
  }
}
