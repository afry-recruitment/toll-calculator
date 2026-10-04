import { Injectable, Logger } from '@nestjs/common';
import { TollEntry } from '../toll/toll-entry.schema.js';
import type { TollTransactionReceipt } from '../toll/types/toll.types.js';

@Injectable()
export class TransportstyrelsenClient {
  private readonly logger = new Logger(TransportstyrelsenClient.name);
  private readonly transactions = new Map<
    string,
    { reference: string; entry: TollEntry }
  >();

  upsertTollTransaction(entry: TollEntry): Promise<TollTransactionReceipt> {
    const existing = this.transactions.get(entry.id);
    const reference =
      existing?.reference ??
      `TS-${String(this.transactions.size + 1).padStart(8, '0')}`;
    this.transactions.set(entry.id, { reference, entry });

    const status = existing ? 'UPDATED' : 'CREATED';
    this.logger.log(
      `[stub] ${status} toll transaction ${entry.id} for ${entry.vehicleNumber} (${reference})`,
    );
    return Promise.resolve({ transactionId: entry.id, reference, status });
  }

  getTollTransaction(transactionId: string): TollEntry | undefined {
    return this.transactions.get(transactionId)?.entry;
  }
}
