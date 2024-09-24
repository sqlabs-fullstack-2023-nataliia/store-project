import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Document, HydratedDocument, Schema as MongooseSchema } from "mongoose";

export type ApiKeyDocument = HydratedDocument<ApiKey>;

@Schema()
export class ApiKey extends Document {

  @Prop({ type: MongooseSchema.Types.ObjectId })
  id: MongooseSchema.Types.ObjectId;

  @Prop({ required: true })
  apiKey: string;

  @Prop({ required: false, default: true })
  isActive?: boolean;
}

export const ApiKeySchema = SchemaFactory.createForClass(ApiKey);
