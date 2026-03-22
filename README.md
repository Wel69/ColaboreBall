# ColaboreBall

**ColaboreBall** é um projeto 2D Unity (URP) que sincroniza duas plataformas virtuais com dois motores Dynamixel reais. O objetivo é gerar bolas coloridas, detectar colisões/erros e enviar comandos de rotação e torque diretamente para o robô em vez de usar UDP.

## Requisitos

- Unity 2020.3 LTS ou superior com Universal RP (essa cena usa URP2D e TextMesh Pro).
- Dynamixel SDK (`dxl_x64_c.dll`) disponível no caminho referenciado por `Assets/Plugins/dynamixel.cs` ou ajuste `dll_path`.
- Dois servos Dynamixel configurados em protocolo 2.0, controláveis via COM (ex.: `COM3`) e IDs `1` e `2`.
- Hardware conectado: plataformas físicas alinhadas com as posições definidas em `Main`.
- IDE (VS/VSCode) com suporte a C# e Git para editar/rodar o projeto.

## Como preparar

1. Clone o repositório:
   ```
   git clone https://github.com/Wel69/ColaboreBall.git
   ```
2. Abra a pasta no Unity Hub e carregue `Assets/Scenes/GameColabore.unity`.
3. No inspetor do `Main` (GameObject vazio), defina:
   - `portName` (ex.: `COM3`)
   - `baudRate` compatível com o servo (57600 ou outro)
   - `platform1Id` e `platform2Id`
   - Endereços (`goalPositionAddress`, `presentPositionAddress`, `torqueEnableAddress`, `torqueLimitAddress`) conforme o modelo.
4. Garanta que `Assets/Plugins/dynamixel.cs` aponte para o `dxl_x64_c.dll` correto. Se estiver em outro diretório, atualize `dll_path`.
5. Conecte o robô ao PC antes de rodar Unity.

## Executar

1. Pressione Play na cena depois que Unity recompilar os scripts.
2. As plataformas virtuais lerão a posição real dos servos e rotacionarão com base nessa posição.
3. As bolas são instanciadas automaticamente; colisões chamam os scripts `ColliderDetection*` para atualizar `ScoreScript.scoreValue`.
4. `WrongPlace1/2` aumenta o limite de torque via `ApplyTorqueLimit` caso alguém jogue fora da área correta.

## Pontuação & detecção

- Bolas amarelas/cian dão pontos para a plataforma esquerda; verdes/magenta/vermelhas para a direita.
- `ScoreScript` atualiza o texto do Canvas com `scoreValue`.
- `CollisionDetected*` é disparado por `ColliderDetectionN` ao tocar cores específicas; as cores especiais (cyan/magenta) aumentam a pontuação.

## Hardware

- Dois servos Dynamixel, torque habilitado, conectados via USB-to-serial.
- A lógica de `Main` alterna entre três posições pré-definidas (`platform1Position` e `platform2Position`) para simular movimentos.
- `ApplyTorqueLimit` recalcula o limite a partir de `kpMX/kpXM` e ajustes são aplicados toda vez que `WrongPlace1/2` é chamado.

## Dicas

- Salve mudanças no Unity antes de commitar; arquivos `.unity`, `.asset` e `.prefab` devem seguir o padrão Unity.
- Caso receba erros de DLL, verifique o path em `dynamixel.cs` e a arquitetura (x64 vs x86).
- Use o console do Unity para monitorar avisos de comunicação e torque.

## Publicar

- Faça commits pequenos e inclua `Assets`/`ProjectSettings`/`Packages`.
- O `.gitignore` já exclui diretórios gerados (`Library`, `Temp`, `.vs` etc.).
