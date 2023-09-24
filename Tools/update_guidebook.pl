#!/usr/bin/perl
use 5.010;
use strict;
use warnings;
use File::Find;

use Cwd qw(getcwd);
#change this to find fields..
my @tagStr = ('solids', 'reagents');
#change this to find file types.
my $fileSubstr = 'meal_recipes.yml';
#change this to change the source directory being searched in.
my $fldrName = "Service";
my $fileNameSrc = 'wiki.txt';
my $fileNameHeader = 'RecipesGeneratedHeader.xml';
my $fileNameDest = "RecipesGenerated.xml";
 
open(SRC, $fileNameSrc) or die $!;
chdir "../";
chdir "Resources";
chdir "ServerInfo";
chdir "Guidebook";
chdir "Service";
open(HDR, $fileNameHeader);
open(DEST, '>', $fileNameDest);
while(<HDR>){
    print DEST;
}
close(HDR);
my $firstIdx = -1;
print DEST "\n";
my $tagIdx = -1;
my $tagType = '';
my $invalidTagType = 'reagents';
while( <SRC> ){
    my $namestr = 'Name';
    my $nameIdx = index( "$_", $namestr );
    if ( $nameIdx != -1 ) {
        if($firstIdx == -1){
            $firstIdx = 0;
        } 
        my $name = "$_";
        s/^\s+//; # Removes leading whitespace.
        my $write = substr $_, 6; #remove 'Name:''
        print DEST $write; 
        print DEST '<Box>';
        print DEST "\n";
    }
    if ( $tagIdx != -1 ){
        
        if( "$_" eq "\n" ){
            $tagIdx = -1;
            print DEST '</Box>';
            print DEST "_\n";
            print DEST "\n";
            print DEST "\n";
        }
        #deal with contents\\
        my $output = "$_";
        $output =~ s/\:.*//; #Removes all text after : (the quantities)
        $output =~ s/^\s+//; #Removes leading whitespace.
        chomp($output); #Removes newline.
        my @memCheck = grep {$_ eq $output} @tagStr;
        if($output && scalar @memCheck == 0 ){
            if( $tagType eq $invalidTagType ){
                $output =~ s/[A-Z][a-z]*+(?!\W)\K/ /gx; #Add a space before each capitalized letter but the first.
                $output = '<GuideEntityEmbed Entity="LargeBeaker" Caption="' . $output . '"/>' . "\n";
            } else {
                $output = '<GuideEntityEmbed Entity="' . $output . '"/>' . "\n";
            }
            print DEST $output;
        }
    }
    #finds ingredients.
    foreach my $interestStr (@tagStr) {
        my $isIdx = index( "$_", $interestStr );
        if( $isIdx != -1){
            $tagIdx = $isIdx;
            $tagType = $interestStr;
        }
    }
}
print DEST '</Box>';
print DEST "\n";
print DEST "\n";
print DEST '</Document>';
print DEST "\n";
close(SRC);
close(DEST);
