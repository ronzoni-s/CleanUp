using CleanUp.Application.Interfaces;
using CleanUp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanUp.Infrastructure.Services
{
    public class GeneticSchedulerService : ISchedulerService
    {
        private class Chromosome
        {
   //         private int[,] genes;
   //         private int fitness;
   //         private List<CleaningOperation> CleaningOperations { get; set; }
   //         private List<CleanUpUser> Operators { get; set; }
   //         private List<Classroom> Classrooms { get; set; }

			//public Chromosome(List<CleaningOperation> cleaningOperations, List<CleanUpUser> operators, List<Classroom> classrooms)
			//{
			//	this.CleaningOperations = cleaningOperations;
			//	this.Operators = operators;
			//	this.Classrooms = classrooms;
			//	this.genes = new int[9, classrooms.Count];
			//	Random r = new Random();
			//	for (int i = 0; i < classrooms.Count(); i++)
			//	{  
			//		// for each slot
			//		//Console.WriteLine("For day " + classrooms.get(i).getDay() + " at " + classrooms.get(i).getHour());
			//		for (int y = 0; y < 8; y = y + 3)
			//		{       
			//			// for each class 
			//			char asciiClass = (char)(65 + y / 3);
			//			int randomOperator = r.Next(operators.Count);
			//			this.genes[y, i] = randomOperator; // operators id
			//			this.genes[y + 1, i] = operators.ElementAt(randomOperator).getCleaningOperation().getId(); // operators cleaningOperation id 
			//			this.genes[y + 2, i] = operators.ElementAt(randomOperator).getCleaningOperation().getClassTeached().getId(); // classes 1-2-3 (ABC)
			//		}
			//	}
			//	this.calculateFitness();
			//}

			////Constructs a copy of a chromosome
			//public Chromosome(int[,] genes)
			//{

			//	this.genes = new int[genes.Count(),genes[0].Count()];
			//	for (int j = 0; j < genes[0].Count(); j++)
			//	{
			//		for (int i = 0; i < genes.Count(); i++)
			//			this.genes[i,j] = genes[i,j];
			//	}
			//	this.calculateFitness();
			//}


			//public int[,] getGenes()
			//{
			//	return this.genes;

			//}
			//public int getGenes(int i, int j)
			//{
			//	return genes[i, j];

			//}

			//public int getFitness()
			//{
			//	return this.fitness;
			//}

			//public void setGenes(int[,] genes)
			//{
			//	for (int i = 0; i < Classrooms.Count(); i++)
			//	{
			//		for (int j = 0; j < 9; j++)
			//			this.genes[i, j] = genes[i, j];
			//	}
			//}

			//public void setFitness(int fitness)
			//{
			//	this.fitness = fitness;
			//}


			//public void calculateFitness()
			//{

			//	int heuristic = 0;
			//	bool different_operators = false;
			//	bool different_cleaningOperations = false;
			//	bool different_class = false;
			//	bool three_hours_straight = false;

			//	for (int j = 0; j < genes[0].Count() - 1; j++)
			//	{   //for each slot

			//		if (genes[0,j] != genes[3,j] && genes[0,j] != genes[6,j] && genes[3,j] != genes[6,j]) different_operators = true;

			//		if (genes[1,j] != genes[4,j] && genes[1,j] != genes[7,j] && genes[4,j] != genes[7,j]) different_cleaningOperations = true;

			//		if (genes[2,j] != genes[5,j] && genes[2,j] != genes[8,j] && genes[5,j] != genes[8,j]) different_class = true;

			//		if (j + 3 < genes[0].Count())
			//		{
			//			if (genes[0,j] == genes[0,j + 1] && genes[0,j + 1] == genes[0,j + 2] && genes[0,j + 2] == genes[0,j + 3]) three_hours_straight = true;
			//		}

			//		if (different_cleaningOperations) heuristic += 25;
			//		if (different_class) heuristic += 22;
			//		if (different_operators) heuristic += 18;
			//		if (!three_hours_straight) heuristic += 15;

			//		different_operators = false;
			//		different_cleaningOperations = false;
			//		different_class = false;
			//		three_hours_straight = false;

			//	}

			//	this.fitness = heuristic;
			//	Console.WriteLine("Fitness: " + heuristic);

			//}


			//public void mutate(List<Teacher> operators)
			//{

			//	Random r = new Random();
			//	int i = r.nextInt(genes[0].Count());
			//	for (int y = 0; y < 8; y = y + 3)
			//	{       // for each class 
			//		int randomTeacher = r.nextInt(operators.size());
			//		this.genes[y,i] = operators.get(randomTeacher).getId(); // operators id
			//		this.genes[y + 1,i] = operators.get(randomTeacher).getCleaningOperation().getId(); // operators cleaningOperation id 
			//		this.genes[y + 2,i] = operators.get(randomTeacher).getCleaningOperation().getClassTeached().getId(); // classes 1-2-3 (ABC)
			//	}
			//	this.calculateFitness();
			//}

			////@Override
			////public int compareTo(Chromosome x)
			////{
			////	return this.fitness - x.fitness;
			////}
		}
    }
}
