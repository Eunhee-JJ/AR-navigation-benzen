
// 4�� ��Ʈ��ŷ
// ��� ���ǽǰ� ������ ���ǽ� Ȥ�� �ܺθ� �Է� �� ��� ��Ʈ��ŷ�� �̿��Ͽ� �����ִ� ��� ��츦 ����մϴ�
// ������ ��θ� ���ϴ� ��� ���Ͽ� �Էµ� ������ ������ ���� ���� ���� ������ ����Դϴ�...
// ������ �ϱ�� �ߴµ� �̰� �´����� �𸣰ڳ׿� Ȥ�� �ʿ��� ����̳� ������� �ʿ��� ��� �������ּż� �ǵ�� ���ֽø� �ذ��ϰڽ��ϴ�.
// ������ ���Ͽ� �ԷµǱ� ������ ������ �ٸ� ���μ������� ����Ϸ��� ����������� ���� ���� �� �� �ֽ��ϴ�!
// version 1.0
using UnityEngine;
using System;

// �׷��� ��� ����ü ����

 public class GraphNode{
	public int vertex;
	public GraphNode link;
};

// �׷��� ���� ����ü ���� (���� ���Ḯ��Ʈ�� ����)
 public class GraphType{
	public int n;
	public GraphNode[] adj_list;
};
 public class Backtracking: MonoBehaviour{

	public int[] result_path = new int[40]; // ***���⿡ �ּ� ��ΰ� ����˴ϴ�!***
	int result_path_count = 100;

	// �׷��� �ʱ�ȭ
	void init(ref GraphType g){
		int v;
		g.n=0;
		// for(v=0;v<100;v++)
		// 	g.adj_list[v]=null;
		g.adj_list = new GraphNode[100];
	}

	// �׷����� ���� �߰�
	void insert_vertex(ref GraphType g ,int v){
		//GraphNode* node;
		GraphNode node = new GraphNode();
		node.vertex = v;
		node.link = null;
		if(((g.n)+1)>100){
			return;
		}
		//node= (GraphNode*)malloc(sizeof(GraphNode));
		//node.vertex = v;
		//node.link = null;
		
		g.adj_list[v] = node;
		g.n++;
	}

	// �׷��� ���� �߰�
	void insert_edge(ref GraphType g,int u,int v){
		GraphNode node = new GraphNode();
		GraphNode start = new GraphNode();
		
		node.vertex=v;
		node.link = null;

		start = g.adj_list[u];
		while(start.link != null)
		{
			start = start.link;
		}
		start.link = node;
	}

	// �׷��� ���
	void print_adj_list(ref GraphType g){
		for(int i=0;i<g.n;i++){
			GraphNode p=g.adj_list[i];
			//printf("���� %d�� ���� ����Ʈ",p.vertex);
			p = p.link;
			while(p!=null){
				// printf(".%d",p.vertex);
				p=p.link;
			}
			//printf("\n");
		}
	}

	// ���ǽ� �Է½� ����(section) ã�� �Լ�
	int Search_Section(int[][] roomnum_list, int room_number) 
	{
		for (int i = 0; i < roomnum_list.Length; i++)
		{
			for (int j = 0; j < roomnum_list[i].Length; j++)
			{
				if (roomnum_list[i][j] == room_number)
				{
					return i; // Ž�� ������ roomnum_list�� ������ȣ ��ȯ
				} 
			}
		}

		return -1; // ���н� -1 ��ȯ
	}

	// ��Ʈ��ŷ �Լ� (�Ű�����)(�׷���, ���۱���, ��������, ������ ���ǽ� ���� �迭, ���İ� ���� ���� �迭, ���İ� ���� ���� �迭 �ε���)
	// ����Լ��� ����
	void _Backtracking(ref GraphType g, int start, int destination, int[][] roomnum_list, int[] path, int path_idx) 
	{
		int[] temp_result = new int[40];
		for(int i = 0; i < 40; i++){
			temp_result[i] = -1;
		}
		if(start == destination) // ���۰� ���� ������
		{
			for(int i = 0; i < 40; i++)
			{	
				if(path[i] == -1)
				{
					temp_result[i] = destination;
					if(result_path_count > i){
						result_path_count = i;
						Buffer.BlockCopy(temp_result, 0, result_path, 0, sizeof(int) * 40);
						//memmove();
					}
					break;
				}
				temp_result[i] = path[i];
			}
			return;
		}
		for (int i = 0; i < 40 ; i++)
		{
			if (path[i] == start)
			{
				return; // �̹� �� ���� ��� ��
			}
		}

		int vid, pid;
		int[] list = new int[40];
		GraphNode temp = new GraphNode();
		pid = path_idx;

		for (int i = 0;i < 40; i++)
		{
			list[i] = path[i];
		}
		temp = g.adj_list[start];

		pid++;
		list[pid] = temp.vertex;

		while(temp.link != null)
		{	
			// �������� �ܺ��� ��찡 �ƴѵ� �ܺ� ��带 ������ �ǳʶٰ� Ž��
			if ((destination != 0) && (temp.link.vertex == 0))
			{
				temp = temp.link;
				continue;
			}
			else
			{
				temp = temp.link;
				_Backtracking(ref g, temp.vertex, destination, roomnum_list, list, pid); // ����� ������ ���� DFS����
			}
		}
	}

	public void FindRoute(int A, int B, out int size)
	{
		// �� ������ ���ǽ� ����
		int[][] roomnum= {
		new int[]{0}, //0=�ܺ� 
		new int[]{31101,31102,31103,31104,31105,31106,31151,31152,31153,31154,31155}, //1=1a
		new int[]{31107,31108,31109,31110,31111,31156,31157}, //2=1b
		new int[]{32101,32102,32103,32104,32151,32152,32153,32154,32156,32157}, //3=1c
		new int[]{32107,32108,32109,32110,32111,32112,32158,32159,32160,32161,32162}, //4=1d
		new int[]{32113,32114,32115,32116,32117,32118,32163,32164,32165}, //5=1e
		new int[]{51101,51102,51103,51104,51151,51152,51153,51154,51155}, //6=1f
		new int[]{51105,51106,51107,51108,51109,51110,51111,51112,51113,51114,51157,51158,51159,51160,51161,51162,51163,51164},  //7=1g
		new int[]{62101,62102,62103,62104,62105,62106,62107,62108,62151,62152,62154,62155,62156}, //8=1h
		new int[]{62110,62111,62157,61101,61102,61103,61104,61151,61152,61153,61153,61154}, //9=1i
		new int[]{61105,61106,61107,61108,61109,61110,61155,61156,61157}, //10=1j, ������� 1�� 
		new int[]{31201,31202,31203,31204,31205,31206,31251,31252,31253,31254,31255}, //11=2a
		new int[]{31207,31208,31209,31210,31211,31212,31215,31256,31257}, //12=2b
		new int[]{31213,31214}, //13=2c
		new int[]{32211,32212,32213,32214,32264,32265,32267,32268}, //14=2d
		new int[]{32203,32204,32205,32206,32207,32208,32209,32210,32256,32257,32258,32260,32262,32263}, //15=2e
		new int[]{32201,32202,32251,32252,32253,32254}, //16=2f
		new int[]{51205,51206,51207}, //17=2g
		new int[]{51201,51203,51204,51251,51252,51253,51255}, //18=2h
		new int[]{51208,51209,51210,51212,51213,51257,51259,51261,51262}, //19=2i
		new int[]{61207,61208,61209}, //20=2j
		new int[]{61201,61202,61203,61205,62209,62210,62211,62212}, //21=2k
		new int[]{62201,62202,62203,62204,62205,62206,62207,62208,62251,62252,62253,62254,62255,62256}, //22=2l
		new int[]{61210,61211,61212,61214,61215,61255,61256}, //23=2m, ������� 2�� 
		new int[]{31301,31302,31303,31304,31305,31306,31351,31352,31353,31354,31355}, //24=3a
		new int[]{31307,31308,313039,31310,31311,31312,31313,31314,31315,31318,31356,31357,31358,31359}, //25=3b
		new int[]{31316,31317}, //26=3c
		new int[]{32311,32312,32313,32316,32317,32362,32363,32364,32365,32366}, //27=3d
		new int[]{32303,32304,32305,32306,32307,32308,32309,32310,32356,32356,32357,32358,32359,32360,32361}, //28=3e
		new int[]{32301,32302,32351,32352,32353,32354,32355}, //29=3f
		new int[]{51306,51307,51308}, //30=3g
		new int[]{51301,51302,51303,51304,51305,51351,51352,51353,51354,51355,51356}, //31=3h
		new int[]{51309,51310,51311,51312,51313,51314,51315,51316,51317,51318,51357,51359,51363}, //32=3i
		new int[]{61305,61306,61307}, //33=3j
		new int[]{61301,61302,61303,61304,61302,62306,62355,61351,61352,61353,61354}, //34=3k
		new int[]{62301,62302,62303,62304,62305,62351,62352,62353,62354}, //35=3l
		new int[]{61308,61309,61310,61311,61312,61313,61314,61315,61316,61355,61356,61357} //36=3m	
		}; 

		for (int i = 0; i < 40; i++){
			result_path[i] = -1;
		}

		int[] P = new int[40];
		int Pidx = -1;
		//int A, B;
		int count = 0;
		GraphType g = new GraphType();
		//g=(GraphType*)malloc(sizeof(GraphType));
		init(ref g);
		for(int i=0;i<37;i++){
			insert_vertex(ref g,i);
		}
		for(int j=1;j<11;j++){
			insert_edge(ref g,0,j);
			insert_edge(ref g,j,0);
		}
		insert_edge(ref g,1,2);
		insert_edge(ref g,3,4);
		insert_edge(ref g,4,5);
		insert_edge(ref g,6,7);
		insert_edge(ref g,8,9);
		insert_edge(ref g,9,10); //������� 1�� ��� 
		insert_edge(ref g,1,11);
		insert_edge(ref g,2,12);
		insert_edge(ref g,2,13);
		insert_edge(ref g,3,16);
		insert_edge(ref g,5,14);
		insert_edge(ref g,7,19);
		insert_edge(ref g,7,20);
		insert_edge(ref g,8,22);
		insert_edge(ref g,10,23); //������� 1~2�� �մ� ���
		insert_edge(ref g,11,12);
		insert_edge(ref g,12,13); 
		insert_edge(ref g,13,14);
		insert_edge(ref g,14,15);
		insert_edge(ref g,14,17);
		insert_edge(ref g,15,16);
		insert_edge(ref g,17,18);
		insert_edge(ref g,17,19);
		insert_edge(ref g,19,20);
		insert_edge(ref g,20,21);
		insert_edge(ref g,20,23); 
		insert_edge(ref g,21,22); //������� 2�� ��� 
		insert_edge(ref g,11,24);
		insert_edge(ref g,12,25);
		insert_edge(ref g,13,26);
		insert_edge(ref g,14,27);
		insert_edge(ref g,16,29);
		insert_edge(ref g,19,32);
		insert_edge(ref g,20,33);
		insert_edge(ref g,22,35);
		insert_edge(ref g,23,36); //������� 2~3�� �մ� ���
		insert_edge(ref g,24,25);
		insert_edge(ref g,25,26);
		insert_edge(ref g,26,27);
		insert_edge(ref g,26,28);
		insert_edge(ref g,28,29);
		insert_edge(ref g,27,30);
		insert_edge(ref g,30,31);
		insert_edge(ref g,30,32);
		insert_edge(ref g,32,33);
		insert_edge(ref g,33,34);
		insert_edge(ref g,34,35);
		insert_edge(ref g,33,36); //������� 3�� ���
		
		insert_edge(ref g,2,1); //�ݴ���� ���� 
		insert_edge(ref g,4,3); 
		insert_edge(ref g,5,4);
		insert_edge(ref g,7,6);
		insert_edge(ref g,9,8);
		insert_edge(ref g,10,9); //������� 1�� ��� 
		insert_edge(ref g,11,1);
		insert_edge(ref g,12,2);
		insert_edge(ref g,13,2);
		insert_edge(ref g,16,3);
		insert_edge(ref g,14,5);
		insert_edge(ref g,19,7);
		insert_edge(ref g,20,7);
		insert_edge(ref g,22,8);
		insert_edge(ref g,23,10); //������� 1~2�� �մ� ���
		insert_edge(ref g,12,11);
		insert_edge(ref g,13,12); 
		insert_edge(ref g,14,13);
		insert_edge(ref g,15,14);
		insert_edge(ref g,17,14);
		insert_edge(ref g,16,15);
		insert_edge(ref g,18,17);
		insert_edge(ref g,19,17);
		insert_edge(ref g,20,19);
		insert_edge(ref g,21,20);
		insert_edge(ref g,23,20); 
		insert_edge(ref g,22,21); //������� 2�� ��� 
		insert_edge(ref g,24,11);
		insert_edge(ref g,25,12);
		insert_edge(ref g,26,13);
		insert_edge(ref g,27,14);
		insert_edge(ref g,29,16);
		insert_edge(ref g,32,19);
		insert_edge(ref g,33,20);
		insert_edge(ref g,35,22);
		insert_edge(ref g,36,23);//������� 2~3�� �մ� ���
		insert_edge(ref g,25,24);
		insert_edge(ref g,26,25);
		insert_edge(ref g,27,26);
		insert_edge(ref g,28,27);
		insert_edge(ref g,29,28);
		insert_edge(ref g,30,27);
		insert_edge(ref g,31,30);
		insert_edge(ref g,32,30);
		insert_edge(ref g,33,32);
		insert_edge(ref g,34,33);
		insert_edge(ref g,35,34);
		insert_edge(ref g,36,33); //������� 3�� ���
		// print_adj_list(g);

		for(int i =0;i<40;i++)
		{
			P[i] = -1; // -1 �� �ʱ�ȭ
		}

		while(true)
		{
			//printf("����� ���ǽǰ� �������� ���ǽ��� �Է��ϼ���( 0 �Է��� ���� �ǹ��մϴ� )(����� ����) : ");
			//scanf("%d %d", &A, &B);
			A = Search_Section(roomnum, A); // ��� ���� ����
			B = Search_Section(roomnum, B); // ���� ���� ����
			if ((A == -1) || (B == -1))
			{
				//printf("\n���ǽ��� �ٽ� �Է��ϼ���!\n\n");
			}
			else 
			{	
				_Backtracking(ref g, A, B, roomnum, P, Pidx);
				while(result_path[count] != -1){
					//printf("%d ", result_path[count]);
					count++;
				}
				//printf("\n��ΰ� ���Ͽ� �Էµƽ��ϴ�! test_back ������ �����ϼ���!\n\n");
				break;
			}
		}
		//system("pause");
		//free(g);
		size = 0;
        for(int i=0; i<result_path.Length && result_path[i] != -1; i++){
            size ++;
        }
		//return *path;
	}

}
