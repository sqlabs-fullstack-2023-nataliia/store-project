import { MongooseModule } from '@nestjs/mongoose';
import { forwardRef, Module } from "@nestjs/common";
import { Account, AccountSchema } from 'src/auth/schemas/account.schema';
import { AuthService } from './services/auth.service';
import { AuthController } from './controllers/auth.controller';
import { JwtModule } from '@nestjs/jwt';
import { ConfigModule } from '@nestjs/config';
import { Profile, ProfileSchema } from '../profile/schemas/profile.schema';
import { ApiKey, ApiKeySchema } from "./schemas/api-key.schema";
import { ApiKeyService } from "./services/api-key.service";
import { ApiKeyController } from "./controllers/api-key.controller";
import { ProfileModule } from "../profile/profile.module";

@Module({
  imports: [
    ConfigModule.forRoot(),
    MongooseModule.forFeature([
      { name: Account.name, schema: AccountSchema },
      { name: Profile.name, schema: ProfileSchema },
      { name: ApiKey.name, schema: ApiKeySchema }
    ]),
    JwtModule.register({
      global: true,
      secret: process.env.TOKEN_KEY,
      signOptions: { expiresIn: '60d' },
    }),
    forwardRef(() => ProfileModule),
  ],
  controllers: [AuthController, ApiKeyController],
  providers: [AuthService, ApiKeyService],
  exports: [AuthService, ApiKeyService],
})
export class AuthModule {}
