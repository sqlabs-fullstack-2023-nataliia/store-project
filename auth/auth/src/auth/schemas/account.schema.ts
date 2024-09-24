import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Document, HydratedDocument, Schema as MongooseSchema } from "mongoose";


export type AccountDocument = HydratedDocument<Account>;

@Schema()
export class Account extends Document {

  @Prop({ type: MongooseSchema.Types.ObjectId })
  id: MongooseSchema.Types.ObjectId;

  @Prop({ required: true })
  email: string;

  @Prop({ required: true })
  password: string;
}

export const AccountSchema = SchemaFactory.createForClass(Account);
