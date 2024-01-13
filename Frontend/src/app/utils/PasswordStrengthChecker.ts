export class PasswordStrengthChecker {
  static calculateEntropy(password: string): number {
    if(!password) return 0;
    let poolSize = 0;

    if (/[a-z]/.test(password)) poolSize += 26;
    if (/[A-Z]/.test(password)) poolSize += 26;
    if (/[0-9]/.test(password)) poolSize += 10;
    if (/[^a-zA-Z0-9]/.test(password)) poolSize += 32;

    return password.length * Math.log2(poolSize);
  }
}
