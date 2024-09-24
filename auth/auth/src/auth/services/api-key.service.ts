import { Injectable, UnauthorizedException } from '@nestjs/common';
import { InjectModel } from '@nestjs/mongoose';
import { Model } from 'mongoose';
import { JwtService } from '@nestjs/jwt';
import { ApiKey } from '../schemas/api-key.schema';

@Injectable()
export class ApiKeyService {
  constructor(
    @InjectModel(ApiKey.name) private apiKeyModel: Model<ApiKey>,
    private readonly jwtService: JwtService,
  ) {}

  async createApiKey(key: string): Promise<ApiKey> {
    const newApiKey = new this.apiKeyModel({ apiKey: key, isActive: true });
    return newApiKey.save();
  }

  async getApiKey(key: string): Promise<ApiKey> {
    return await this.apiKeyModel.findOne({ key });
  }

  // Method to validate the API key and generate JWT token
  async validateApiKeyAndGenerateToken(
    apiKey: string,
  ): Promise<{ accessToken: string }> {
    const keyRecord = await this.apiKeyModel.findOne({
      apiKey: apiKey,
      isActive: true,
    });

    if (!keyRecord) {
      throw new UnauthorizedException('Invalid or inactive API Key');
    }

    // Customize payload as needed, here using the apiKey itself
    const payload = {
      apiKey: apiKey,
      isActive: true,
      aud: 'test',
      iss: 'test',
    };
    const accessToken = this.jwtService.sign(payload);

    return { accessToken };
  }
}
