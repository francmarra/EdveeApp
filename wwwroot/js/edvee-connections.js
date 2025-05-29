// EdVee Connection Management JavaScript
// Extracted from EdVee.cshtml for better organization

// Variáveis para controle de ligações
let connectionMode = false;
let deleteMode = false;
let selectedElement = null;
let lines = [];
let currentConnections = [];

// Função para posicionar corretamente os pontos de conexão
function positionConnectionPoints() {
    document.querySelectorAll('.connectable').forEach(element => {
        // Pega o ID do elemento conectável
        const id = element.dataset.id;
        const section = element.dataset.section;
        
        // Encontra os pontos de conexão relacionados
        const connectionPoints = document.querySelectorAll(`.connection-point[data-for="${id}"]`);
        
        connectionPoints.forEach(point => {
            // Ajusta a posição vertical para alinhar com o elemento
            const elementRect = element.getBoundingClientRect();
            const containerRect = point.parentElement.getBoundingClientRect();
            
            // Calcula a posição vertical relativa ao contêiner
            // Usamos altura exata dividida por 2 para garantir o centro exato
            const top = elementRect.top - containerRect.top + (elementRect.height / 2) - (point.offsetHeight / 2);
            
            // Define a posição do ponto
            point.style.top = `${top}px`;
        });
    });
}

// Função para recuperar as ligações existentes do servidor
async function loadConnections(ucId) {
    try {
        const response = await fetch(`/UC/GetConnections/${ucId}`);
        if (response.ok) {
            const connections = await response.json();
            currentConnections = connections; // Armazena as conexões recuperadas
            renderConnections(connections);
        }
    } catch (error) {
        console.error('Erro ao carregar conexões:', error);
    }
}

// Função para salvar as conexões criadas
async function saveConnections(ucId) {
    try {
        // Garantir que os IDs sejam números (eles vêm como strings do DOM)
        const connectionsToSave = currentConnections.map(conn => ({
            origemTipo: conn.origemTipo,
            origemId: conn.origemId.toString(), // Garantir que é string (o controller fará a conversão)
            destinoTipo: conn.destinoTipo,
            destinoId: conn.destinoId.toString() // Garantir que é string (o controller fará a conversão)
        }));
        
        console.log('Enviando conexões:', connectionsToSave);
        
        const response = await fetch('/UC/SaveConnections', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                ucId: ucId,
                connections: connectionsToSave
            })
        });
        
        const result = await response.json();
        
        if (response.ok) {
            showCustomPopup('Conexões salvas com sucesso!', 'success');
        } else {
            showCustomPopup('Erro ao salvar conexões: ' + (result.message || 'Erro desconhecido'), 'error');
            console.error('Resposta de erro:', result);
        }
    } catch (error) {
        console.error('Erro ao salvar conexões:', error);
        showCustomPopup('Erro ao salvar conexões. Veja o console para detalhes.', 'error');
    }
}

// Função para mostrar pop-up personalizado
function showCustomPopup(message, type = 'info') {
    // Remove pop-up existente se houver
    const existingPopup = document.querySelector('.custom-popup');
    if (existingPopup) {
        existingPopup.remove();
    }

    // Cria o overlay
    const overlay = document.createElement('div');
    overlay.className = 'popup-overlay';
    
    // Cria o pop-up
    const popup = document.createElement('div');
    popup.className = `custom-popup ${type}`;
    
    // Define o ícone baseado no tipo
    let icon = '';
    if (type === 'success') {
        icon = '✓';
    } else if (type === 'error') {
        icon = '✕';
    } else {
        icon = 'ℹ';
    }
    
    popup.innerHTML = `
        <div class="popup-content">
            <div class="popup-icon">${icon}</div>
            <div class="popup-message">${message}</div>
            <button class="popup-close-btn" onclick="closeCustomPopup()">OK</button>
        </div>
    `;
    
    // Adiciona ao documento
    overlay.appendChild(popup);
    document.body.appendChild(overlay);
    
    // Adiciona evento para fechar clicando fora do pop-up
    overlay.addEventListener('click', (e) => {
        if (e.target === overlay) {
            closeCustomPopup();
        }
    });

    // Adiciona evento para fechar com a tecla Escape
    const handleEscape = (e) => {
        if (e.key === 'Escape') {
            closeCustomPopup();
            document.removeEventListener('keydown', handleEscape);
        }
    };
    document.addEventListener('keydown', handleEscape);
    
    // Mostra com animação
    setTimeout(() => {
        overlay.classList.add('show');
    }, 10);
    
    // Auto-close após 4 segundos para mensagens de sucesso
    if (type === 'success') {
        setTimeout(() => {
            closeCustomPopup();
        }, 4000);
    }
}

// Função para fechar o pop-up personalizado
function closeCustomPopup() {
    const popup = document.querySelector('.popup-overlay');
    if (popup) {
        popup.classList.remove('show');
        setTimeout(() => {
            popup.remove();
        }, 300);
    }
}

// Função para desenhar as ligações na tela
function renderConnections(connections) {
    // Limpa as linhas existentes
    clearLines();
    
    // Para cada conexão, cria uma linha
    connections.forEach(conn => {
        const origemElement = document.querySelector(`[data-id="${conn.origemTipo}-${conn.origemId}"]`);
        const destinoElement = document.querySelector(`[data-id="${conn.destinoTipo}-${conn.destinoId}"]`);
        
        if (origemElement && destinoElement) {
            // Encontra os pontos de conexão externos
            const origemPoint = document.querySelector(`.connection-point[data-for="${conn.origemTipo}-${conn.origemId}"]`);
            const destinoPoint = document.querySelector(`.connection-point[data-for="${conn.destinoTipo}-${conn.destinoId}"]`);
            
            if (origemPoint && destinoPoint) {
                // Define a cor da linha baseada nos tipos de origem e destino
                let lineColor = getLineColor(origemElement.dataset.section, destinoElement.dataset.section);
                
                // Cria uma linha simples usando os pontos de conexão externos
                const line = new LeaderLine(
                    origemPoint,
                    destinoPoint,
                    {
                        path: 'straight',
                        endPlug: 'behind',
                        startPlug: 'behind',
                        size: 2.5,
                        color: lineColor,
                        startSocket: 'right',
                        endSocket: 'left',
                        startSocketGravity: 0,  // Sem curva na saída
                        endSocketGravity: 0,    // Sem curva na chegada
                        startPlugSize: 1, // Tamanho reduzido para melhor alinhamento
                        endPlugSize: 1,    // Tamanho reduzido para melhor alinhamento
                        dash: {animation: true}
                    }
                );
                lines.push(line);
            }
        }
    });
}

// Função para determinar a cor da linha baseada nos tipos de origem e destino
function getLineColor(origemSection, destinoSection) {
    // Cor padrão
    let color = '#0066cc';
    
    // Cores por tipo de seção
    if ((origemSection === 'Conteudo' && destinoSection === 'Atividade') || 
        (origemSection === 'Atividade' && destinoSection === 'Conteudo')) {
        color = '#4caf50'; // Verde para conexões Conteudo-Atividade
    } else if ((origemSection === 'Competencia' && destinoSection === 'Avaliacao') || 
              (origemSection === 'Avaliacao' && destinoSection === 'Competencia')) {
        color = '#2196f3'; // Azul para conexões Competencia-Avaliacao
    } else if ((origemSection === 'Competencia' && destinoSection === 'Atividade') || 
              (origemSection === 'Atividade' && destinoSection === 'Competencia')) {
        color = '#e91e63'; // Rosa para conexões Competencia-Atividade
    } else if ((origemSection === 'Conteudo' && destinoSection === 'Avaliacao') || 
              (origemSection === 'Avaliacao' && destinoSection === 'Conteudo')) {
        color = '#ffc107'; // Amarelo para conexões Conteudo-Avaliacao
    }
    
    return color;
}

// Função para limpar todas as linhas
function clearLines() {
    lines.forEach(line => line.remove());
    lines = [];
}

// Função para limpar tudo (linhas e conexões)
function clearAllConnections() {
    lines.forEach(line => line.remove());
    lines = [];
    currentConnections = [];
}

// Função para ativar/desativar o modo de conexão
function toggleConnectionMode() {
    connectionMode = !connectionMode;
    const button = document.getElementById('toggleConnectionMode');
    const instructions = document.querySelector('.connection-instructions');
    const deleteBtn = document.getElementById('deleteIndividualConnection');
    const clearBtn = document.getElementById('clearConnections');
    const saveBtn = document.getElementById('saveConnections');
    
    if (connectionMode) {
        button.textContent = 'Modo de Conexão: LIGADO';
        button.classList.add('active');
        button.classList.remove('inactive');
        instructions.style.display = 'block';
        deleteBtn.style.display = 'inline-block';
        clearBtn.style.display = 'inline-block';
        saveBtn.style.display = 'inline-block';
        
        // Desativa modo de eliminação se estiver ativo
        if (deleteMode) {
            deleteMode = false;
            const deleteModeBtn = document.getElementById('deleteIndividualConnection');
            deleteModeBtn.textContent = 'Eliminar Conexão Individual';
            deleteModeBtn.style.backgroundColor = '#f44336';
            
            // Remove classes de modo de eliminação
            document.querySelectorAll('.connection-point').forEach(el => {
                el.classList.remove('delete-mode');
                el.classList.remove('selected-element');
            });
            
            // Restaura aparência normal das linhas
            restoreNormalConnections();
        }
        
        // Adiciona classe de destaque aos pontos de conexão
        document.querySelectorAll('.connection-point').forEach(el => {
            el.classList.add('connection-mode');
        });
        
        // Atualiza os tooltips
        setConnectionPointTooltips();
    } else {
        button.textContent = 'Modo de Conexão: DESLIGADO';
        button.classList.remove('active');
        button.classList.add('inactive');
        instructions.style.display = 'none';
        deleteBtn.style.display = 'none';
        clearBtn.style.display = 'none';
        saveBtn.style.display = 'none';
        selectedElement = null;
        deleteMode = false;
        
        // Remove classe de destaque
        document.querySelectorAll('.connection-point').forEach(el => {
            el.classList.remove('connection-mode');
            el.classList.remove('selected-element');
            el.classList.remove('can-connect');
            el.classList.remove('delete-mode');
        });
        
        // Atualiza os tooltips
        setConnectionPointTooltips();
        
        // Renderiza novamente as conexões existentes (sem perder dados)
        renderStoredConnections();
    }
}

// Função para ativar/desativar o modo de eliminação individual
function toggleDeleteMode() {
    deleteMode = !deleteMode;
    const button = document.getElementById('deleteIndividualConnection');
    
    if (deleteMode) {
        button.textContent = 'Cancelar Eliminação';
        button.style.backgroundColor = '#ff9800';
        connectionMode = false; // Desativa modo de conexão
        selectedElement = null;
        
        // Remove classes dos pontos de conexão
        document.querySelectorAll('.connection-point').forEach(el => {
            el.classList.remove('connection-mode');
            el.classList.remove('selected-element');
            el.classList.remove('can-connect');
            el.classList.add('delete-mode'); // Adiciona classe para modo de eliminação
        });
        
        // Muda a aparência das linhas para indicar que podem ser eliminadas
        highlightDeletableConnections();
        
        setConnectionPointTooltips();
    } else {
        button.textContent = 'Eliminar Conexão Individual';
        button.style.backgroundColor = '#f44336';
        selectedElement = null;
        
        // Remove classes de modo de eliminação
        document.querySelectorAll('.connection-point').forEach(el => {
            el.classList.remove('delete-mode');
            el.classList.remove('selected-element');
        });
        
        // Restaura aparência normal das linhas
        restoreNormalConnections();
    }
}

// Função para eliminar uma conexão específica entre dois pontos
function deleteConnectionBetweenPoints(ponto1, ponto2) {
    console.log('Procurando conexão entre:', ponto1.dataset.for, 'e', ponto2.dataset.for);
    
    const [tipo1, id1] = ponto1.dataset.for.split('-');
    const [tipo2, id2] = ponto2.dataset.for.split('-');
    
    // Procura a conexão nos dois sentidos possíveis
    let connectionIndex = -1;
    
    connectionIndex = currentConnections.findIndex(conn => 
        (conn.origemTipo === tipo1 && conn.origemId == id1 && 
         conn.destinoTipo === tipo2 && conn.destinoId == id2) ||
        (conn.origemTipo === tipo2 && conn.origemId == id2 && 
         conn.destinoTipo === tipo1 && conn.destinoId == id1)
    );
    
    if (connectionIndex !== -1) {
        console.log(`Conexão encontrada no índice ${connectionIndex}:`, currentConnections[connectionIndex]);
        
        // Remove a linha visual
        const lineToRemove = lines[connectionIndex];
        lineToRemove.remove();
        lines.splice(connectionIndex, 1);
        
        // Remove da lista de conexões
        currentConnections.splice(connectionIndex, 1);
        
        console.log(`Conexão eliminada. Restam ${lines.length} linhas e ${currentConnections.length} conexões`);
        
        // Restaura aparência das linhas restantes
        if (deleteMode) {
            highlightDeletableConnections();
        }
        
        showCustomPopup('Conexão eliminada com sucesso!', 'success');
        return true;
    } else {
        console.log('Nenhuma conexão encontrada entre estes pontos');
        showCustomPopup('Não existe conexão entre estes pontos!', 'error');
        return false;
    }
}

// Função para destacar conexões que podem ser eliminadas
function highlightDeletableConnections() {
    lines.forEach(line => {
        line.setOptions({
            size: 4,
            color: '#ff5722',
            dash: {animation: true, len: 8, gap: 4}
        });
    });
}

// Função para restaurar aparência normal das conexões
function restoreNormalConnections() {
    lines.forEach((line, index) => {
        const conn = currentConnections[index];
        if (conn) {
            const origemElement = document.querySelector(`[data-id="${conn.origemTipo}-${conn.origemId}"]`);
            const destinoElement = document.querySelector(`[data-id="${conn.destinoTipo}-${conn.destinoId}"]`);
            if (origemElement && destinoElement) {
                const originalColor = getLineColor(origemElement.dataset.section, destinoElement.dataset.section);
                line.setOptions({
                    size: 2.5,
                    color: originalColor,
                    dash: {animation: true}
                });
            }
        }
    });
}

// Função para renderizar as conexões armazenadas sem limpar a lista atual
function renderStoredConnections() {
    // Limpa apenas as linhas visuais, mantendo as conexões armazenadas
    lines.forEach(line => line.remove());
    lines = [];
    
    // Para cada conexão armazenada, cria uma linha visual
    currentConnections.forEach(conn => {
        const origemElement = document.querySelector(`[data-id="${conn.origemTipo}-${conn.origemId}"]`);
        const destinoElement = document.querySelector(`[data-id="${conn.destinoTipo}-${conn.destinoId}"]`);
        
        if (origemElement && destinoElement) {
            // Encontra os pontos de conexão externos
            const origemPoint = document.querySelector(`.connection-point[data-for="${conn.origemTipo}-${conn.origemId}"]`);
            const destinoPoint = document.querySelector(`.connection-point[data-for="${conn.destinoTipo}-${conn.destinoId}"]`);
            
            if (origemPoint && destinoPoint) {
                let lineColor = getLineColor(origemElement.dataset.section, destinoElement.dataset.section);
                
                // Cria a linha visual
                const line = new LeaderLine(
                    origemPoint,
                    destinoPoint,
                    {
                        path: 'straight',
                        endPlug: 'behind',
                        startPlug: 'behind',
                        size: 2.5, // Linha um pouco mais espessa para melhor visualização
                        color: lineColor,
                        startSocket: 'right',
                        endSocket: 'left',
                        startSocketGravity: 0,
                        endSocketGravity: 0,
                        startPlugSize: 1, // Tamanho reduzido para melhor alinhamento
                        endPlugSize: 1,    // Tamanho reduzido para melhor alinhamento
                        dash: {animation: true}
                    }
                );
                lines.push(line);
            }
        }
    });
    
    // Se estamos em modo de eliminação, aplica os estilos visuais
    if (deleteMode) {
        highlightDeletableConnections();
    }
}

// Função para limpar o estado de "pode conectar" de todos os pontos
function clearCanConnectStatus() {
    document.querySelectorAll('.connection-point').forEach(el => {
        el.classList.remove('can-connect');
    });
}

// Função para mostrar pontos que podem ser conectados
function showConnectablePotentials(selectedPoint) {
    if (!selectedPoint) return;
    
    // Determinar os tipos compatíveis com base no selecionado
    const [tipo, id] = selectedPoint.dataset.for.split('-');
    const section = selectedPoint.dataset.section;
    
    // Diferentes regras de conexão baseadas no tipo
    let compatibleSections = [];
    
    if (section === "Competencia") {
        compatibleSections = ["Conteudo", "Avaliacao", "Atividade"];
    } else if (section === "Conteudo") {
        compatibleSections = ["Avaliacao", "Atividade"];
    } else if (section === "Avaliacao") {
        compatibleSections = ["Atividade"];
    }
    
    // Adicionar a classe aos pontos compatíveis
    document.querySelectorAll('.connection-point').forEach(el => {
        if (el === selectedPoint) return; // Pula o próprio ponto
        
        const pointSection = el.dataset.section;
        if (compatibleSections.includes(pointSection)) {
            el.classList.add('can-connect');
        }
    });
}

// Manipulador de clique para selecionar elementos
function handleConnectionPointClick(e) {
    const element = e.currentTarget;
    const connectableId = element.dataset.for;
    
    // Se estamos em modo de eliminação
    if (deleteMode) {
        if (!selectedElement) {
            // Primeira seleção para eliminação
            selectedElement = element;
            element.classList.add('selected-element');
            setConnectionPointTooltips();
        } else if (selectedElement === element) {
            // Clicou no mesmo elemento - deseleciona
            selectedElement.classList.remove('selected-element');
            selectedElement = null;
            setConnectionPointTooltips();
        } else {
            // Segunda seleção - tenta eliminar conexão
            const sucesso = deleteConnectionBetweenPoints(selectedElement, element);
            
            // Limpa a seleção independentemente do resultado
            selectedElement.classList.remove('selected-element');
            selectedElement = null;
            setConnectionPointTooltips();
        }
        return;
    }
    
    // Lógica original para modo de conexão
    if (!connectionMode) return;
    
    if (!selectedElement) {
        // Primeira seleção
        selectedElement = element;
        element.classList.add('selected-element');
        showConnectablePotentials(element); // Mostrar pontos que podem ser conectados
        setConnectionPointTooltips(); // Atualizar tooltips
    } else if (selectedElement === element) {
        // Clicou no mesmo elemento - deseleciona
        selectedElement.classList.remove('selected-element');
        selectedElement = null;
        clearCanConnectStatus(); // Limpar estado de "pode conectar"
        setConnectionPointTooltips(); // Atualizar tooltips
    } else {
        // Segunda seleção - cria a conexão
        const origem = selectedElement;
        const destino = element;
        
        // Busca os IDs dos elementos conectáveis
        const origemConnectableId = origem.dataset.for;
        const destinoConnectableId = destino.dataset.for;
        
        // Extrai tipo e ID
        const [origemTipo, origemId] = origemConnectableId.split('-');
        const [destinoTipo, destinoId] = destinoConnectableId.split('-');
        
        // Encontra os elementos conectáveis para usar seus datasets
        const origemElement = document.querySelector(`[data-id="${origemConnectableId}"]`);
        const destinoElement = document.querySelector(`[data-id="${destinoConnectableId}"]`);
        
        // Verifica se a conexão já existe
        const existingConnection = currentConnections.find(
            conn => conn.origemId == origemId && 
                   conn.origemTipo === origemTipo &&
                   conn.destinoId == destinoId &&
                   conn.destinoTipo === destinoTipo
        );
        
        if (!existingConnection && origemElement && destinoElement) {
            // Define a cor da linha baseada nas seções
            let lineColor = getLineColor(origemElement.dataset.section, destinoElement.dataset.section);
            
            // Cria a linha visual simples
            const line = new LeaderLine(
                origem,
                destino,
                {
                    path: 'straight',
                    endPlug: 'behind',
                    startPlug: 'behind',
                    size: 2.5,
                    color: lineColor,
                    startSocket: 'right',
                    endSocket: 'left',
                    startSocketGravity: 0,
                    endSocketGravity: 0,
                    startPlugSize: 1, // Tamanho reduzido para melhor alinhamento
                    endPlugSize: 1,   // Tamanho reduzido para melhor alinhamento
                    dash: {animation: true}
                }
            );
            lines.push(line);
            
            // Adiciona à lista de conexões
            currentConnections.push({
                origemTipo: origemTipo,
                origemId: origemId,
                destinoTipo: destinoTipo,
                destinoId: destinoId
            });
            
            // Exibe feedback visual
            showConnectionFeedback(origem, destino);
        }
        
        // Limpa a seleção
        selectedElement.classList.remove('selected-element');
        selectedElement = null;
        clearCanConnectStatus(); // Limpar estado de "pode conectar"
        setConnectionPointTooltips(); // Atualizar tooltips
    }
}

// Função para exibir feedback visual quando uma conexão é criada
function showConnectionFeedback(origem, destino) {
    // Adiciona uma classe temporária para indicar sucesso
    [origem, destino].forEach(el => {
        el.classList.add('connection-success');
        
        // Remove a classe após a animação
        setTimeout(() => {
            el.classList.remove('connection-success');
        }, 1000);
    });
}

// Função para definir os tooltips para os pontos de conexão
function setConnectionPointTooltips() {
    document.querySelectorAll('.connection-point').forEach(point => {
        const [tipo, id] = point.dataset.for.split('-');
        const section = point.dataset.section;
        
        let tooltipText = "";
        if (deleteMode) {
            if (selectedElement === point) {
                tooltipText = "Clique para cancelar seleção";
            } else if (selectedElement) {
                tooltipText = "Clique para eliminar conexão";
            } else {
                tooltipText = "Clique para selecionar ponto de eliminação";
            }
        } else if (connectionMode) {
            if (selectedElement === point) {
                tooltipText = "Clique para cancelar";
            } else if (selectedElement) {
                tooltipText = "Clique para conectar";
            } else {
                tooltipText = "Clique para selecionar";
            }
        } else {
            tooltipText = `${section} (Ponto de conexão)`;
        }
        
        point.setAttribute('data-tooltip', tooltipText);
    });
}

// Add topic form functionality
function initializeAddTopicForms() {
    // Add topic form toggle functionality
    document.querySelectorAll('.add-topic-btn').forEach(btn => {
        btn.addEventListener('click', function() {
            const targetId = this.getAttribute('data-target');
            const targetForm = document.getElementById(targetId);
            
            // Efeito visual ao clicar no botão
            this.style.transform = 'scale(0.98)';
            setTimeout(() => {
                this.style.transform = 'scale(1)';
            }, 150);
            
            // Close any open forms first
            document.querySelectorAll('.add-topic-form.active').forEach(openForm => {
                if (openForm.id !== targetId) {
                    openForm.classList.remove('active');
                    setTimeout(() => {
                        openForm.querySelector('textarea').value = ''; // Clear the input
                    }, 300);
                }
            });
            
            // Toggle the target form
            targetForm.classList.toggle('active');
            
            // Focus on the textarea if form is now active
            if (targetForm.classList.contains('active')) {
                setTimeout(() => {
                    targetForm.querySelector('textarea').focus();
                }, 100);
            }
        });
    });
    
    // Cancel button functionality
    document.querySelectorAll('.btn-cancel').forEach(btn => {
        btn.addEventListener('click', function() {
            const form = this.closest('.add-topic-form');
            
            // Efeito visual ao clicar no botão
            this.style.transform = 'scale(0.95)';
            setTimeout(() => {
                this.style.transform = 'scale(1)';
            }, 150);
            
            form.classList.remove('active');
            
            // Clear the input with delay
            setTimeout(() => {
                form.querySelector('textarea').value = '';
            }, 300);
        });
    });
    
    // Confirm button visual feedback
    document.querySelectorAll('.btn-confirm').forEach(btn => {
        btn.addEventListener('mousedown', function() {
            this.style.transform = 'scale(0.95)';
        });
        
        btn.addEventListener('mouseup', function() {
            this.style.transform = 'scale(1)';
        });
        
        btn.addEventListener('mouseleave', function() {
            this.style.transform = 'scale(1)';
        });
    });
}

// Initialization function that will be called from the view
function initializeEdVee(ucId) {
    // Posiciona os pontos de conexão
    positionConnectionPoints();
    
    // Definir tooltips iniciais
    setConnectionPointTooltips();
    
    // Carrega conexões existentes
    loadConnections(ucId);
    
    // Configura handlers dos botões
    document.getElementById('toggleConnectionMode').addEventListener('click', toggleConnectionMode);
    document.getElementById('deleteIndividualConnection').addEventListener('click', toggleDeleteMode);
    document.getElementById('clearConnections').addEventListener('click', () => {
        clearAllConnections();
        if (!connectionMode) {
            renderStoredConnections();
        }
    });
    document.getElementById('saveConnections').addEventListener('click', () => saveConnections(ucId));
    
    // Adiciona handler de clique aos pontos de conexão
    document.querySelectorAll('.connection-point').forEach(el => {
        el.addEventListener('click', handleConnectionPointClick);
    });
    
    // Ajusta as linhas quando a janela é redimensionada
    window.addEventListener('resize', () => {
        positionConnectionPoints();
        lines.forEach(line => line.position());
    });
    
    // Observer para mudanças no layout
    const observer = new MutationObserver(() => {
        positionConnectionPoints();
        lines.forEach(line => line.position());
    });
    
    // Observa mudanças no layout
    observer.observe(document.body, { 
        childList: true, 
        subtree: true,
        attributes: true
    });
    
    // Initialize add topic forms
    initializeAddTopicForms();
}
