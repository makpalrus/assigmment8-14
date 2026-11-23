const fs = require('node:fs');
const path = require('node:path');
const out = path.resolve(__dirname, '../Assets/Resources/Audio');
const rate = 22050;
function tone(name, seconds, start, end, pulse, noise = 0) {
  const count = Math.floor(seconds * rate);
  const wav = Buffer.alloc(44 + count * 2);
  wav.write('RIFF'); wav.writeUInt32LE(wav.length - 8, 4); wav.write('WAVEfmt ', 8);
  wav.writeUInt32LE(16, 16); wav.writeUInt16LE(1, 20); wav.writeUInt16LE(1, 22);
  wav.writeUInt32LE(rate, 24); wav.writeUInt32LE(rate * 2, 28);
  wav.writeUInt16LE(2, 32); wav.writeUInt16LE(16, 34); wav.write('data', 36); wav.writeUInt32LE(count * 2, 40);
  let phase = 0, seed = 12345;
  for (let i = 0; i < count; i++) {
    const t = i / rate, p = i / count;
    phase += 2 * Math.PI * (start + (end - start) * p) / rate;
    seed = (1664525 * seed + 1013904223) >>> 0;
    const envelope = Math.min(1, t / .008) * Math.min(1, (seconds - t) / .04);
    const tremolo = pulse ? .55 + .45 * Math.cos(2 * Math.PI * pulse * t) : 1;
    const value = .43 * envelope * tremolo * ((1 - noise) * (Math.sin(phase) + .2 * Math.sin(2 * phase)) / 1.2 + noise * (seed / 2147483648 - 1));
    wav.writeInt16LE(Math.round(Math.max(-1, Math.min(1, value)) * 32767), 44 + 2 * i);
  }
  fs.writeFileSync(path.join(out, name + '.wav'), wav);
}
tone('land', .12, 130, 65, 0, .4);
tone('heal', .35, 500, 1050, 12);
tone('death', .5, 400, 70, 7);
tone('patrolwarning', .48, 220, 330, 8);
tone('shooterwarning', .48, 650, 1100, 11);
tone('chargerwarning', .55, 100, 270, 14, .18);
tone('strike', .16, 200, 60, 0, .5);
tone('shoot', .18, 1200, 250, 0);
tone('dash', .3, 140, 700, 0, .55);
const hurt = fs.readFileSync(path.join(out, 'hurt.wav'));
for (const n of [2, 3]) {
  const wav = Buffer.from(hurt), count = (wav.length - 44) / 2;
  for (let i = 0; i < count; i++) {
    const source = Math.min(count - 1, Math.floor(i * (n === 2 ? .88 : 1.12)));
    const value = hurt.readInt16LE(44 + source * 2) * Math.min(1, (count - i) / 500) * (n === 2 ? .85 : .95);
    wav.writeInt16LE(Math.round(value), 44 + i * 2);
  }
  fs.writeFileSync(path.join(out, 'hurt_' + n + '.wav'), wav);
}
