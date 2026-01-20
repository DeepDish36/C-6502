using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace C_6502
{
    internal class CPU6502
    {
        public Action<int, byte> OnVideoWrite;

        // Registos do 6502
        public byte A, X, Y;      // Acumulador e registos X, Y
        public byte SP;           // Stack Pointer
        public ushort PC = 0x0600;         // Program Counter (default $0600)
        public byte Status;       // Flags (N, V, -, B, D, I, Z, C)



        // Memória (64KB)
        public byte[] Memory = new byte[65536];

        // Inicializa a CPU
        public CPU6502()
        {
            Reset();

            // Garante que, ao criar a CPU pela primeira vez, o PC comece em $0600
            // (útil enquanto o vetor de reset na memória não estiver inicializado)
            if (PC == 0)
                PC = 0x0600;
        }

        // Reset: Define valores iniciais
        public void Reset()
        {
            // Lê o vetor de reset; se ainda não foi definido, usa o endereço padrão $0600
            ushort resetVector = ReadWord(0xFFFC);
            PC = resetVector != 0 ? resetVector : (ushort)0x0600;
            SP = 0xFD;
            Status = 0x30; // Bit 5 sempre 1, Bit 2 sempre 1
        }

        // Lê um byte da memória
        public byte ReadByte(ushort address)
        {
            return Memory[address];
        }

        // Lê duas bytes (16 bits) da memória
        private ushort ReadWord(ushort addr)
        {
            byte low = ReadByte(addr);
            byte high = ReadByte((ushort)(addr + 1));
            return (ushort)((high << 8) | low);
        }

        //public ushort ReadWord(ushort address)
        //{
        //    return (ushort)(Memory[address] | (Memory[address + 1] << 8));
        //}

        // Escreve um byte na memória
        public void WriteByte(ushort address, byte value)
        {
            Memory[address] = value;

            // Se for endereço de vídeo (de $0200 a $05FF), notifica o ecrã
            if (address >= 0x0200 && address < 0x0600)
            {
                OnVideoWrite?.Invoke(address, value);
            }
        }

        //---------------------------------------------------------------------------------//

        // Executa uma instrução
        /// <summary>
        /// Executes a single instruction of the 6502 CPU.
        /// </summary>
        public void Step()
        {
            byte opcode = ReadByte(PC++);
            switch (opcode)
            {
                //Instruções de opcode 0x00 a 0xF0
                case 0x00: // BRK - Break
                    BRK();
                    break;

                case 0x10: // BPL (Branch if Negative flag is clear)
                    BPL();
                    break;

                case 0x20: // JSR (Jump to SubRoutine)
                    JSR();
                    break;

                case 0x30: // BMI (Branch if Positive flag is clear) 
                    BMI();
                    break;

                case 0x40: // RTI (ReTurn from Interrupt)
                    RTI();
                    break;

                case 0x50:
                    BVC();
                    break;

                case 0x60:
                    RTS();
                    break;

                case 0x70:
                    BVS();
                    break;

                case 0x90:
                    BCC();
                    break;

                case 0xA0: // LDY #imediato
                    Y = ReadByte(PC++);
                    SetZeroNegativeFlags(Y);
                    break;

                case 0xB0: // BCS (Branch if Carry Set)
                    BCS();
                    break;

                case 0xC0:
                    CPY_Immediate();
                    break;

                case 0xD0: // BNE (Branch if Not Equal)
                    BNE_Relative();
                    break;

                case 0xE0: // CPX (Compare X Register)
                    CPX_Immediate();
                    break;

                case 0xF0: // BEQ (Branch if Equal)
                    BEQ_Relative();
                    break;

                //---------------------------------------------------------------------------------//

                // Instruções de opcode 0x01 a 0xF1
                case 0x01: // ORA (Logical OR Accumulator)
                    ORA_IndirectX();
                    break;

                case 0x11:
                    ORA_IndirectY();
                    break;

                case 0x21:
                    AND_IndirectX();
                    break;

                case 0x31:
                    AND_IndirectY();
                    break;

                case 0x41:
                    EOR_IndirectX();
                    break;

                case 0x51:
                    EOR_IndirectY();
                    break;

                case 0x61:
                    ADC_IndirectX();
                    break;

                case 0x71:
                    ADC_IndirectY();
                    break;

                case 0x81:
                    STA_IndirectX();
                    break;

                case 0x91:
                    STA_IndirectY();
                    break;

                case 0xA1: // LDA (Load Accumulator) com endereçamento indireto X
                    LDA_IndirectX();
                    break;

                case 0xB1:
                    LDA_IndirectY();
                    break;

                case 0xC1: // CMP (Compare) com endereçamento indireto X
                    CMP_IndirectX();
                    break;

                case 0xD1:
                    CMP_IndirectY();
                    break;

                case 0xE1: // SBC (Subtract with Carry) com endereçamento indireto X
                    SBC_IndirectX();
                    break;

                case 0xF1:
                    SBC_IndirectY();
                    break;

                //---------------------------------------------------------------------------------//
                //Instrução de opcode 0xA2
                case 0xA2: // LDX #imediato
                    LDX_Immediate();
                    break;

                //---------------------------------------------------------------------------------//
                //Instruções de opcode 0x24 a 0xE4    
                case 0x24: // BIT Zero Page
                    {
                        BIT_ZPG();
                        break;
                    }
                case 0x84: // STY Zero Page
                    {
                        STY_ZPG();
                        break;
                    }
                case 0x94: // STY Zero Page,X
                    {
                        STY_ZPG_X();
                        break;
                    }
                case 0xA4: // LDY Zero Page
                    {
                        LDY_ZPG();
                        break;
                    }
                case 0xB4: // LDY Zero Page,X
                    {
                        LDY_ZPG_X();
                        break;
                    }
                case 0xC4: // CPY Zero Page
                    {
                        CPY_ZPG();
                        break;
                    }
                case 0xE4: // CPX Zero Page
                    {
                        CPX_ZPG();
                        break;
                    }
                // Instruções de opcode 0x05 a 0xF5
                case 0x05: 
                    ORA_ZeroPage(); 
                    break;
                
                case 0x15: 
                    ORA_ZeroPageX(); 
                    break;
                
                case 0x25: 
                    AND_ZeroPage(); 
                    break;
                
                case 0x35: 
                    AND_ZeroPageX(); 
                    break;
                
                case 0x45: 
                    EOR_ZeroPage(); 
                    break;
                
                case 0x55: 
                    EOR_ZeroPageX(); 
                    break;
                
                case 0x65: 
                    ADC_ZeroPage(); 
                    break;
                
                case 0x75: 
                    ADC_ZeroPageX(); 
                    break;
                
                case 0x85: 
                    STA_ZeroPage(); 
                    break;
                
                case 0x95: 
                    STA_ZeroPageX(); 
                    break;
                
                case 0xA5: 
                    LDA_ZeroPage(); 
                    break;
                
                case 0xB5: 
                    LDA_ZeroPageX(); 
                    break;
                
                case 0xC5: 
                    CMP_ZeroPage(); 
                    break;
                
                case 0xD5: 
                    CMP_ZeroPageX(); 
                    break;
                
                case 0xE5: 
                    SBC_ZeroPage(); 
                    break;
                
                case 0xF5: 
                    SBC_ZeroPageX(); 
                    break;

                // Intruções de opcode 0x06 a 0xF6
                case 0x06: 
                    ASL_ZeroPage(); 
                    break;
                
                case 0x16: 
                    ASL_ZeroPageX(); 
                    break;
                
                case 0x26: 
                    ROL_ZeroPage(); 
                    break;
                
                case 0x36: 
                    ROL_ZeroPageX(); 
                    break;
                
                case 0x46: 
                    LSR_ZeroPage(); 
                    break;
                
                case 0x56: 
                    LSR_ZeroPageX(); 
                    break;
                
                case 0x66: 
                    ROR_ZeroPage(); 
                    break;
                
                case 0x76: 
                    ROR_ZeroPageX(); 
                    break;
                
                case 0x86: 
                    STX_ZeroPage(); 
                    break;
                
                case 0x96: 
                    STX_ZeroPageY(); 
                    break;
                
                case 0xA6: 
                    LDX_ZeroPage(); 
                    break;
                
                case 0xB6: 
                    LDX_ZeroPageY(); 
                    break;
                
                case 0xC6: 
                    DEC_ZeroPage(); 
                    break;
                
                case 0xD6: 
                    DEC_ZeroPageX(); 
                    break;
                
                case 0xE6: 
                    INC_ZeroPage(); 
                    break;
                
                case 0xF6: 
                    INC_ZeroPageX(); 
                    break;
                //

                case 0xA9: // LDA #imediato
                    A = ReadByte(PC++);
                    SetZeroNegativeFlags(A);
                    break;

                case 0x8D: // STA abs
                    STA();
                    break;

                default:
                    Console.WriteLine($"Opcode desconhecido: {opcode:X2}");
                    break;
            }
            Debugger.PrintState(this);
            Debugger.PrintHexDump(this, 0x0600, 32);
        }

        //---------------------------------------------------------------------------------//

        //Métodos para executar as instruções

        //Instruções de controle de fluxo
        //Instruções de 00 a F0
        //Executa a instrução BRK para interromper o programa
        private void BRK()
        {
            PC++; // O BRK usa 2 bytes, então avançamos o PC antes de empilhar

            // Empilha PC (LSB primeiro, depois MSB)
            PushStack((byte)(PC >> 8));
            PushStack((byte)(PC & 0xFF));

            // Define a flag Break (bit 4) e empilha o status
            Status |= 0x10;
            PushStack(Status);

            // Lê o vetor de interrupção (endereço 0xFFFE-0xFFFF) e salta para lá
            PC = ReadWord(0xFFFE);
        }

        // Executa a instrução BPL (Branch if Positive)
        private void BPL()
        {
            sbyte offset = (sbyte)ReadByte(PC++);
            if ((Status & 0x80) == 0)
                PC = (ushort)(PC + offset);
        }

        //Executa a instrução JSR (Jump to Subroutine)
        private void JSR()
        {
            ushort address = ReadWord(PC);
            PC += 2;

            // Push high byte depois low byte
            PushByte((byte)((PC - 1) >> 8));  // PC - 1 conforme comportamento do 6502
            PushByte((byte)(PC - 1));

            PC = address;
        }

        // Executa a instrução BMI (Branch if Minus)
        private void BMI()
        {
            sbyte offset = (sbyte)ReadByte(PC++); // Offset relativo pode ser negativo
            if ((Status & 0x80) != 0)             // Se o bit N (bit 7) estiver setado
            {
                PC = (ushort)(PC + offset);
            }
        }

        //Executa a instrução RTI
        private void RTI()
        {
            Status = PullStack();
            byte low = PullStack();
            byte high = PullStack();
            PC = (ushort)((high << 8) | low);
        }

        // Executa a instrução BVC (Branch if Overflow Clear)
        private void BVC()
        {
            sbyte offset = (sbyte)ReadByte(PC++);
            if ((Status & 0x40) == 0) // V flag is 0
            {
                PC = (ushort)(PC + offset);
            }
        }

        //Executa a instrução RTS (ReTurn from Subroutine)
        private void RTS()
        {
            ushort returnAddress = (ushort)(PullStack() | (PullStack() << 8));
            PC = (ushort)(returnAddress + 1);
        }

        private void BVS()
        {
            sbyte offset = (sbyte)ReadByte(PC++);
            if ((Status & 0x40) != 0) // Verifica se flag V está ativa
            {
                PC = (ushort)(PC + offset);
            }
        }

        private void BCC()
        {
            sbyte offset = (sbyte)ReadByte(PC++); // offset relativo com sinal
            if ((Status & 0x01) == 0) // Verifica se o bit C (carry) está limpo
            {
                PC = (ushort)(PC + offset);
            }
        }

        private void BCS()
        {
            sbyte offset = (sbyte)ReadByte(PC++);
            if ((Status & 0x01) != 0) // Se o bit Carry estiver ativo
                PC = (ushort)(PC + offset);
        }

        private void CPY_Immediate()
        {
            byte value = ReadByte(PC++);
            SetCompareFlags(Y, value);
        }

        private void BNE_Relative()
        {
            sbyte offset = (sbyte)ReadByte(PC++);
            if ((Status & 0x02) == 0) // Zero flag is clear
            {
                PC = (ushort)(PC + offset);
            }
        }

        private void CPX_Immediate()
        {
            byte value = ReadByte(PC++);
            SetCompareFlags(X, value);
        }

        private void BEQ_Relative()
        {
            sbyte offset = (sbyte)ReadByte(PC++);
            if ((Status & 0x02) != 0) // Zero flag is set
            {
                PC = (ushort)(PC + offset);
            }
        }


        //---------------------------------------------------------------------------------//

        //Instruções de carga e armazenamento
        //Instruções 01 até F1

        // ORA (Logical OR) com endereçamento indireto X
        /// <summary>
        /// Executes the ORA instruction with indirect addressing mode using the X register.
        /// </summary>
        private void ORA_IndirectX()
        {
            byte zpAddr = ReadByte(PC++);
            ushort addr = (ushort)((zpAddr + X) & 0xFF); // Zero Page wraparound
            ushort effectiveAddr = (ushort)(
                ReadByte(addr) |
                (ReadByte((ushort)((addr + 1) & 0xFF)) << 8)
            );
            byte value = ReadByte(effectiveAddr);

            A |= value;
            SetZeroNegativeFlags(A);
        }

        // ORA (Logical OR) com endereçamento indireto Y
        /// <summary>
        /// Executes the ORA instruction with indirect addressing mode using the Y register.
        /// </summary>
        private void ORA_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort baseAddr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((ushort)((zpAddr + 1) & 0xFF)) << 8)
            );
            ushort effectiveAddr = (ushort)(baseAddr + Y);

            byte value = ReadByte(effectiveAddr);
            A |= value;
            SetZeroNegativeFlags(A);
        }

        // AND (Logical AND) com endereçamento indireto X
        /// <summary>
        /// Executes the AND instruction with indirect addressing mode using the X register.
        /// </summary>
        private void AND_IndirectX()
        {
            byte zpAddr = ReadByte(PC++);
            ushort addr = (ushort)((zpAddr + X) & 0xFF);
            ushort effectiveAddr = (ushort)(
                ReadByte(addr) |
                (ReadByte((ushort)((addr + 1) & 0xFF)) << 8)
            );
            byte value = ReadByte(effectiveAddr);
            A &= value;
            SetZeroNegativeFlags(A);
        }

        // AND (Logical AND) com endereçamento indireto Y
        /// <summary>
        /// Executes the AND instruction with indirect addressing mode using the Y register.
        /// </summary>
        private void AND_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort baseAddr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((byte)((zpAddr + 1) & 0xFF)) << 8)
            );
            ushort effectiveAddr = (ushort)(baseAddr + Y);
            byte value = ReadByte(effectiveAddr);
            A &= value;
            SetZeroNegativeFlags(A);
        }

        // EOR (Exclusive OR) com endereçamento indireto X
        /// <summary>
        /// Executes the EOR instruction with indirect addressing mode using the X register.
        /// </summary>
        private void EOR_IndirectX()
        {
            byte zpAddr = (byte)(ReadByte(PC++) + X);
            ushort effectiveAddr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((byte)((zpAddr + 1) & 0xFF)) << 8)
            );
            byte value = ReadByte(effectiveAddr);
            A ^= value;
            SetZeroNegativeFlags(A);
        }

        // EOR (Exclusive OR) com endereçamento indireto Y
        /// <summary>
        /// Executes the EOR instruction with indirect addressing mode using the Y register.
        /// </summary>
        private void EOR_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort baseAddr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((byte)((zpAddr + 1) & 0xFF)) << 8)
            );
            ushort effectiveAddr = (ushort)(baseAddr + Y);
            byte value = ReadByte(effectiveAddr);
            A ^= value;
            SetZeroNegativeFlags(A);
        }

        // ADC (Add with Carry) com endereçamento indireto X
        /// <summary>
        /// Executes the ADC instruction with indirect addressing mode using the X register.
        /// </summary>
        private void ADC_IndirectX()
        {
            byte zpAddr = ReadByte(PC++);
            ushort pointer = (ushort)((zpAddr + X) & 0xFF);
            ushort effectiveAddr = (ushort)(ReadByte(pointer) | (ReadByte((ushort)((pointer + 1) & 0xFF)) << 8));
            byte value = ReadByte(effectiveAddr);
            ADC(value);
        }

        // ADC (Add with Carry) com endereçamento indireto Y
        /// <summary>
        /// Executes the ADC instruction with indirect addressing mode using the Y register.
        /// </summary>
        private void ADC_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort baseAddr = (ushort)(ReadByte(zpAddr) | (ReadByte((ushort)((zpAddr + 1) & 0xFF)) << 8));
            ushort effectiveAddr = (ushort)(baseAddr + Y);
            byte value = ReadByte(effectiveAddr);
            ADC(value);
        }

        // STA (Store Accumulator) com endereçamento absoluto
        /// <summary>
        /// Stores the value of the accumulator (A) into memory at the specified address.
        /// </summary>
        private void STA()
        {
            ushort addr = ReadWord(PC);
            PC += 2;
            WriteByte(addr, A);
        }

        // STA (Store Accumulator) com endereçamento indireto X
        /// <summary>
        /// Stores the value of the accumulator (A) into memory at the address specified by the zero page address plus the X register.
        /// </summary>
        private void STA_IndirectX()
        {
            byte zpAddr = ReadByte(PC++);
            ushort pointer = (ushort)((zpAddr + X) & 0xFF);
            ushort effectiveAddr = (ushort)(
                ReadByte((ushort)(pointer)) |
                (ReadByte((ushort)((pointer + 1) & 0xFF)) << 8)
            );

            WriteByte(effectiveAddr, A);

            // Se estiver na memória de vídeo, aciona atualização do pixel
            if (effectiveAddr >= 0x0200 && effectiveAddr <= 0x05FF)
                OnVideoWrite?.Invoke((ushort)(effectiveAddr - 0x0200), A);
        }

        // STA (Store Accumulator) com endereçamento indireto Y
        /// <summary>
        /// Stores the value of the accumulator (A) into memory at the address specified by the zero page address plus the Y register.
        /// </summary>
        private void STA_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort baseAddr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((byte)((zpAddr + 1) & 0xFF)) << 8)
            );
            ushort effectiveAddr = (ushort)(baseAddr + Y);

            WriteByte(effectiveAddr, A);

            // Se for área de vídeo, atualiza o pixel
            if (effectiveAddr >= 0x0200 && effectiveAddr <= 0x05FF)
                OnVideoWrite?.Invoke((ushort)(effectiveAddr - 0x0200), A);
        }

        // LDA (Load Accumulator) com endereçamento indireto X
        /// <summary>
        /// Loads a byte from memory into the accumulator (A) using indirect addressing mode with the X register.
        /// </summary>
        private void LDA_IndirectX()
        {
            byte zpAddr = ReadByte(PC++);
            byte effectiveZP = (byte)(zpAddr + X); // wrap-around automático

            ushort addr = (ushort)(
                ReadByte(effectiveZP) |
                (ReadByte((byte)((effectiveZP + 1) & 0xFF)) << 8)
            );

            A = ReadByte(addr);
            SetZeroNegativeFlags(A);
        }

        // LDA (Load Accumulator) com endereçamento indireto Y
        /// <summary>
        /// Loads a byte from memory into the accumulator (A) using indirect addressing mode with the Y register.
        /// </summary>
        /// <param name="value"></param>
        private void LDA_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort addr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((byte)((zpAddr + 1) & 0xFF)) << 8)
            );

            addr += Y;
            A = ReadByte(addr);
            SetZeroNegativeFlags(A);
        }

        // CMP (Compare) com endereçamento indireto X
        /// <summary>
        /// Compares the value in the accumulator (A) with a byte from memory using indirect addressing mode with the X register.
        /// </summary>
        private void CMP_IndirectX()
        {
            byte zpAddr = ReadByte(PC++);
            byte effectiveZP = (byte)(zpAddr + X);

            ushort addr = (ushort)(
                ReadByte(effectiveZP) |
                (ReadByte((byte)((effectiveZP + 1) & 0xFF)) << 8)
            );

            byte value = ReadByte(addr);
            SetCompareFlags(A, value);
        }

        // CMP (Compare) com endereçamento indireto Y
        /// <summary>
        /// Compares the value in the accumulator (A) with a byte from memory using indirect addressing mode with the Y register.
        /// </summary>

        private void CMP_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort addr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((byte)((zpAddr + 1) & 0xFF)) << 8)
            );

            addr += Y;
            byte value = ReadByte(addr);
            SetCompareFlags(A, value);
        }

        // SBC (Subtract with Carry) com endereçamento indireto X
        /// <summary>
        /// Subtracts a byte from the accumulator (A) with carry using indirect addressing mode with the X register.
        /// </summary>

        private void SBC_IndirectX()
        {
            byte zpAddr = ReadByte(PC++);
            byte effectiveZP = (byte)(zpAddr + X);

            ushort addr = (ushort)(
                ReadByte(effectiveZP) |
                (ReadByte((byte)((effectiveZP + 1) & 0xFF)) << 8)
            );

            byte value = ReadByte(addr);
            SBC_Operation(value);
        }

        // SBC (Subtract with Carry) com endereçamento indireto Y
        /// <summary>
        /// Subtracts a byte from the accumulator (A) with carry using indirect addressing mode with the Y register.
        /// </summary>

        private void SBC_IndirectY()
        {
            byte zpAddr = ReadByte(PC++);
            ushort addr = (ushort)(
                ReadByte(zpAddr) |
                (ReadByte((byte)((zpAddr + 1) & 0xFF)) << 8)
            );

            addr += Y;
            byte value = ReadByte(addr);
            SBC_Operation(value);
        }

        private void LDX_Immediate()
        {
            byte value = ReadByte(PC++);
            X = value;
            SetFlag(0x02, X == 0);           // Z
            SetFlag(0x80, (X & 0x80) != 0);  // N
        }

        //---------------------------------------------------------------------------------//

        //Instruções 24 até E4

        private void BIT_ZPG()
        {
            byte addr = ReadByte(PC++);
            byte value = ReadByte(addr);
            byte result = (byte)(A & value);
            SetZeroFlag(result == 0);
            SetOverflowFlag((value & 0x40) != 0); // Bit 6
            SetNegativeFlag((value & 0x80) != 0); // Bit 7
            PC += 2;
        }

        private void STY_ZPG()
        {
            byte addr = ReadByte(PC++);
            WriteByte(addr, Y);
            PC += 2;
        }

        private void STY_ZPG_X()
        {
            byte addr = (byte)(ReadByte(PC++) + X); // wrap-around 0xFF
            WriteByte(addr, Y);
            PC += 2;
        }

        private void LDY_ZPG()
        {
            byte addr = ReadByte(PC++);
            Y = ReadByte(addr);
            SetZeroFlag(Y == 0);
            SetNegativeFlag((Y & 0x80) != 0);
            PC += 2;
        }

        private void LDY_ZPG_X()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            Y = ReadByte(addr);
            SetZeroFlag(Y == 0);
            SetNegativeFlag((Y & 0x80) != 0);
            PC += 2;
        }

        private void CPY_ZPG()
        {
            byte addr = ReadByte(PC++);
            byte value = ReadByte(addr);
            byte result = (byte)(Y - value);
            SetCarryFlag(Y >= value);
            SetZeroFlag(Y == value);
            SetNegativeFlag((result & 0x80) != 0);
            PC += 2;
        }

        private void CPX_ZPG()
        {
            byte addr = ReadByte(PC++);
            byte value = ReadByte(addr);
            byte result = (byte)(X - value);
            SetCarryFlag(X >= value);
            SetZeroFlag(X == value);
            SetNegativeFlag((result & 0x80) != 0);
            PC += 2;
        }

        //---------------------------------------------------------------------------------//

        // Instruções 05 até F5

        // === ORA ===
        private void ORA_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            A |= ReadByte(addr);
            SetFlag(0x02, A == 0);          // Z
            SetFlag(0x80, (A & 0x80) != 0); // N
        }

        private void ORA_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            A |= ReadByte(addr);
            SetFlag(0x02, A == 0);
            SetFlag(0x80, (A & 0x80) != 0);
        }

        // === AND ===
        private void AND_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            A &= ReadByte(addr);
            SetFlag(0x02, A == 0);
            SetFlag(0x80, (A & 0x80) != 0);
        }

        private void AND_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            A &= ReadByte(addr);
            SetFlag(0x02, A == 0);
            SetFlag(0x80, (A & 0x80) != 0);
        }

        // === EOR ===
        private void EOR_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            A ^= ReadByte(addr);
            SetFlag(0x02, A == 0);
            SetFlag(0x80, (A & 0x80) != 0);
        }

        private void EOR_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            A ^= ReadByte(addr);
            SetFlag(0x02, A == 0);
            SetFlag(0x80, (A & 0x80) != 0);
        }

        // === ADC ===
        private void ADC_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            ADC(ReadByte(addr));
        }

        private void ADC_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            ADC(ReadByte(addr));
        }

        // === SBC ===
        private void SBC_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            SBC_Operation(ReadByte(addr));
        }

        private void SBC_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            SBC_Operation(ReadByte(addr));
        }

        // === STA ===
        private void STA_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            WriteByte(addr, A);
        }

        private void STA_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            WriteByte(addr, A);
        }

        // === LDA ===
        private void LDA_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            A = ReadByte(addr);
            SetFlag(0x02, A == 0);
            SetFlag(0x80, (A & 0x80) != 0);
        }

        private void LDA_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            A = ReadByte(addr);
            SetFlag(0x02, A == 0);
            SetFlag(0x80, (A & 0x80) != 0);
        }

        // === CMP ===
        private void CMP_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            SetCompareFlags(A, ReadByte(addr));
        }

        private void CMP_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            SetCompareFlags(A, ReadByte(addr));
        }

        //----------------------------------------------------------------------------------//

        // Instruções 06 até F6
        public void ASL_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            byte value = ReadByte(addr);
            // Use the existing helper to set the carry flag
            SetCarryFlag((value & 0x80) != 0);
            value <<= 1;
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
        }

        public void ASL_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            byte value = ReadByte(addr);
            // Use the existing helper to set the carry flag
            SetCarryFlag((value & 0x80) != 0);
            value <<= 1;
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
        }

        public void ROL_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            byte value = ReadByte(addr);
            bool oldCarry = GetCarryFlag();
            SetCarryFlag((value & 0x80) != 0);
            value = (byte)((value << 1) | (oldCarry ? 1 : 0));
            WriteByte(addr, value);
            SetZeroFlag(value == 0);
            SetNegativeFlag((value & 0x80) != 0);
        }

        public void ROL_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            byte value = ReadByte(addr);
            bool oldCarry = GetCarryFlag();
            SetCarryFlag((value & 0x80) != 0);
            value = (byte)((value << 1) | (oldCarry ? 1 : 0));
            WriteByte(addr, value);
            SetZeroFlag(value == 0);
            SetNegativeFlag((value & 0x80) != 0);
        }

        public void LSR_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            byte value = ReadByte(addr);
            SetCarryFlag((value & 0x01) != 0);
            value >>= 1;
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
        }

        public void LSR_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            byte value = ReadByte(addr);
            SetCarryFlag((value & 0x01) != 0);
            value >>= 1;
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
        }

        public void ROR_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            byte value = ReadByte(addr);
            bool oldCarry = GetCarryFlag();
            SetCarryFlag((value & 0x01) != 0);
            value = (byte)((value >> 1) | (oldCarry ? 0x80 : 0));
            WriteByte(addr, value);
            SetZeroFlag(value == 0);
            SetNegativeFlag((value & 0x80) != 0);
        }

        public void ROR_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            byte value = ReadByte(addr);
            bool oldCarry = GetCarryFlag();
            SetCarryFlag((value & 0x01) != 0);
            value = (byte)((value >> 1) | (oldCarry ? 0x80 : 0));
            WriteByte(addr, value);
            SetZeroFlag(value == 0);
            SetNegativeFlag((value & 0x80) != 0);
        }

        public void STX_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            WriteByte(addr, X);
            PC += 2;
        }

        public void STX_ZeroPageY()
        {
            byte addr = (byte)(ReadByte(PC++) + Y);
            WriteByte(addr, X);
            PC += 2;
        }

        public void LDX_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            X = ReadByte(addr);
            SetZeroFlag(X == 0);
            SetNegativeFlag((X & 0x80) != 0);
            PC += 2;
        }

        public void LDX_ZeroPageY()
        {
            byte addr = (byte)(ReadByte(PC++) + Y);
            X = ReadByte(addr);
            SetZeroFlag(X == 0);
            SetNegativeFlag((X & 0x80) != 0);
            PC += 2;
        }

        public void DEC_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            byte value = (byte)(ReadByte(addr) - 1);
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
            PC += 2;
        }

        public void DEC_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            byte value = (byte)(ReadByte(addr) - 1);
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
            PC += 2;
        }

        public void INC_ZeroPage()
        {
            byte addr = ReadByte(PC++);
            byte value = (byte)(ReadByte(addr) + 1);
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
            PC += 2;
        }

        public void INC_ZeroPageX()
        {
            byte addr = (byte)(ReadByte(PC++) + X);
            byte value = (byte)(ReadByte(addr) + 1);
            WriteByte(addr, value);
            SetZeroNegativeFlags(value);
            PC += 2;
        }

        //----------------------------------------------------------------------------------//

        //Comandos auxiliares
        // Empilha o valor no stack
        private void PushStack(byte value)
        {
            Memory[0x0100 + SP] = value;
            SP--;
        }

        // Atualiza os flags Z (Zero) e N (Negativo)
        private void SetZeroNegativeFlags(byte value)
        {
            Status = (byte)((Status & 0x7D) | (value == 0 ? 0x02 : 0) | (value & 0x80));
        }

        void SetZeroFlag(bool value) => Status = value ? (byte)(Status | 0x02) : (byte)(Status & ~0x02);

        void SetNegativeFlag(bool value) => Status = value ? (byte)(Status | 0x80) : (byte)(Status & ~0x80);

        void SetCarryFlag(bool value) => Status = value ? (byte)(Status | 0x01) : (byte)(Status & ~0x01);

        bool GetCarryFlag() => (Status & 0x01) != 0;

        void SetOverflowFlag(bool value) => Status = value ? (byte)(Status | 0x40) : (byte)(Status & ~0x40);


        // Carrega um programa na memória e define o PC
        public void LoadProgram(byte[] program, ushort startAddress = 0x0600)
        {
            Array.Clear(Memory, 0, Memory.Length);
            for (int i = 0; i < program.Length; i++)
            {
                ushort addr = (ushort)(startAddress + i);
                Memory[addr] = program[i];

                // Notifica se o endereço for da área de vídeo
                if (addr >= 0x0200 && addr <= 0x05FF)
                {
                    OnVideoWrite?.Invoke(addr, program[i]);
                }
            }

            // Define o vetor de reset para apontar para o início do programa
            Memory[0xFFFC] = (byte)(startAddress & 0xFF);        // low byte
            Memory[0xFFFD] = (byte)((startAddress >> 8) & 0xFF); // high byte

            // Atualiza PC a partir do vetor de reset para iniciar o programa carregado
            PC = ReadWord(0xFFFC);

            // NÃO definir PC aqui
            // PC = startAddress;
        }

        // Empilha um byte no stack
        private void PushByte(byte value)
        {
            Memory[0x0100 + SP] = value;
            SP--;
        }

        // Desempilha um byte do stack
        private byte PopByte()
        {
            SP++;
            return Memory[0x0100 + SP];
        }

        private byte PullStack()
        {
            SP++;
            return ReadByte((ushort)(0x0100 + SP));
        }

        private void ADC(byte value)
        {
            int carry = (Status & 0x01);
            int result = A + value + carry;

            // Set flags
            SetFlag(0x01, result > 0xFF); // Carry
            SetFlag(0x02, (result & 0xFF) == 0); // Zero
            SetFlag(0x80, (result & 0x80) != 0); // Negative

            // Overflow: se sinais dos operandos forem iguais e o sinal do resultado for diferente
            bool overflow = (~(A ^ value) & (A ^ result) & 0x80) != 0;
            SetFlag(0x40, overflow); // Overflow

            A = (byte)(result & 0xFF);
        }

        private void SetFlag(byte flag, bool value)
        {
            if (value)
                Status |= flag;
            else
                Status &= (byte)~flag;
        }

        private void SetCompareFlags(byte reg, byte value)
        {
            byte result = (byte)(reg - value);
            SetZeroNegativeFlags(result);

            // Set Carry se A >= M (sem overflow)
            if (reg >= value)
                Status |= 0x01; // C
            else
                Status &= 0xFE; // Clear C
        }

        private void SBC_Operation(byte value)
        {
            int carry = (Status & 0x01) != 0 ? 1 : 0;
            int result = A - value - (1 - carry);

            // Set flags
            SetFlag(0x01, result >= 0); // Carry
            byte result8 = (byte)result;
            SetZeroNegativeFlags(result8);

            // Overflow flag: se o sinal mudar de forma inesperada
            bool overflow = ((A ^ result8) & 0x80) != 0 && ((A ^ value) & 0x80) != 0;
            SetFlag(0x40, overflow); // V

            A = result8;
        }

        public string DisassembleSingle(ref ushort pc)
        {
            ushort start = pc;
            byte opcode = ReadByte(pc++);
            byte b1 = 0, b2 = 0;
            ushort addr;
            string result;
            string comment = "";

            switch (opcode)
            {
                case 0xA9: b1 = ReadByte(pc++); result = $"LDA #${b1:X2}"; comment = $"Carrega o valor ${b1:X2} no acumulador"; break;
                case 0x8D:
                    b1 = ReadByte(pc++);
                    b2 = ReadByte(pc++);
                    addr = (ushort)(b1 | (b2 << 8));
                    result = $"STA ${addr:X4}";
                    comment = $"Guarda o acumulador em ${addr:X4}";
                    break;
                case 0x00:
                    result = "BRK";
                    comment = "Interrupção/terminar";
                    break;
                default:
                    result = $"??? (${opcode:X2})";
                    break;
            }

            StringBuilder bytes = new StringBuilder();
            for (ushort i = start; i < pc; i++)
                bytes.Append($"{ReadByte(i):X2} ");

            string line = $"${start:X4}    {bytes.ToString().PadRight(9)} {result}";
            if (!string.IsNullOrEmpty(comment))
                line = $"{line.PadRight(32)} ; {comment}";

            return line;
        }
    }
}
